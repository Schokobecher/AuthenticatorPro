﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AuthenticatorPro.Shared.Data;
using SQLite;

namespace AuthenticatorPro.Data.Backup.Converter
{
    internal class AuthenticatorPlusBackupConverter : BackupConverter
    {
        public override BackupPasswordPolicy PasswordPolicy => BackupPasswordPolicy.Always;

        public override async Task<Backup> Convert(byte[] data, string password = null)
        {
            var path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                "authplus.db"
            );

            await File.WriteAllBytesAsync(path, data);
            
            var connStr = new SQLiteConnectionString(path, true, password);
            var connection = new SQLiteAsyncConnection(connStr);

            Backup backup;

            try
            {
                var sourceAccounts = await connection.QueryAsync<Account>("SELECT * FROM accounts");
                var sourceCategories = await connection.QueryAsync<Category>("SELECT * FROM category");

                var authenticators = sourceAccounts.Select(account => account.Convert()).ToList();
                var categories = sourceCategories.Select(category => category.Convert()).ToList();
                var bindings = new List<AuthenticatorCategory>();

                for(var i = 0; i < sourceAccounts.Count; ++i)
                {
                    var sourceAccount = sourceAccounts[i];

                    if(sourceAccount.CategoryName == "All Accounts")
                        continue;

                    var category = categories.FirstOrDefault(c => c.Name == sourceAccount.CategoryName);

                    if(category == null)
                        continue;

                    var auth = authenticators[i];
                    var binding = new AuthenticatorCategory
                    {
                        AuthenticatorSecret = auth.Secret, CategoryId = category.Id
                    };

                    bindings.Add(binding);
                }

                backup = new Backup(authenticators, categories, bindings);
            }
            catch(SQLiteException)
            {
                throw new ArgumentException("Database cannot be opened");
            }
            finally
            {
                await connection.CloseAsync();
                File.Delete(path);
            }

            return backup;
        }

        private enum Type
        {
            Totp = 0, Hotp = 1
        }

        private class Account
        {
            [Column("email")]
            public string Email { get; set; }
           
            [Column("secret")]
            public string Secret { get; set; }
            
            [Column("counter")]
            public int Counter { get; set; }
            
            [Column("type")]
            public Type Type { get; set; }

            [Column("issuer")]
            public string Issuer { get; set; }
           
            [Column("category")]
            public string CategoryName { get; set; }

            public Authenticator Convert()
            {
                var type = Type switch
                {
                    Type.Totp => AuthenticatorType.Totp,
                    Type.Hotp => AuthenticatorType.Hotp,
                    _ => throw new ArgumentOutOfRangeException()
                };

                string issuer;
                string username = null;
                string icon = null;

                if(Issuer == "")
                    issuer = Email;
                else
                {
                    issuer = Issuer;
                    username = Email;
                    icon = Icon.FindServiceKeyByName(Issuer);
                }

                return new Authenticator
                {
                    Issuer = issuer,
                    Username = username,
                    Type = type,
                    Secret = Authenticator.CleanSecret(Secret, type),
                    Counter = Counter,
                    Icon = icon
                };
            }
        }

        private class Category
        {
            [Column("name")]
            public string Name { get; set; }
            
            public Data.Category Convert()
            {
                return new(Name);
            }
        }
    }
}