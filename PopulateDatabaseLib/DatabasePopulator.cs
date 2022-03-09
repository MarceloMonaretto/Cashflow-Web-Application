using AccountRepositoryLib.Repositories;
using Microsoft.AspNetCore.Builder;
using ModelsLib.ContextRepositoryClasses;
using RoleRepositoryLib.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionRepositoryLib.Data;

namespace PopulateDatabaseLib
{
    public class DatabasePopulator : IDatabasePopulator
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ITransactionRepository _transactionRepository;

        public DatabasePopulator(IAccountRepository accountRepository,
            IRoleRepository roleRepository, ITransactionRepository transactionRepository)
        {
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
            _transactionRepository = transactionRepository;
        }

        public DatabasePopulator(IAccountRepository accountRepository,
            IRoleRepository roleRepository)
        {
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
            _transactionRepository = null;
        }

        public DatabasePopulator(ITransactionRepository transactionRepository)
        {
            _accountRepository = null;
            _roleRepository = null;
            _transactionRepository = transactionRepository;
        }

        public async Task PopulateAccountsAsync()
        {
            var accounts = GenerateAccountEntries();
            var areThereAccounts = (await _accountRepository.GetAllAccountsAsync()).Any();
            if (!areThereAccounts)
            {
                foreach (var account in accounts)
                {
                    await _accountRepository.CreateAccountAsync(account);
                }
            }
        }

        public async Task PopulateRolesAsync()
        {
            var roles = GenerateRoleEntries();
            var areThereRoles = (await _roleRepository.GetAllRolesAsync()).Any();
            if (!areThereRoles)
            {
                foreach (var role in roles)
                {
                    await _roleRepository.CreateRoleAsync(role);
                }
            }
        }

        public async Task PopulateTransitionsAsync()
        {
            var transactions = GenerateTransactionEntries();
            var areThereTransactions = (await _transactionRepository.GetAllTransactionsAsync()).Any();
            if (!areThereTransactions)
            {
                foreach (var transaction in transactions)
                {
                    await _transactionRepository.CreateTransactionAsync(transaction);
                }
            }
        }

        private static IEnumerable<Transaction> GenerateTransactionEntries()
        {
            List<Transaction> transactions = new();
            string paymentType;
            int month = 2;
            int day = 6;
            int hour = 0;
            int minute = 0;
            int second = 0;

            for (int transactionNumber = 1; transactionNumber <= 30; transactionNumber++)
            {
                if (transactionNumber % 2 == 0)
                    paymentType = "Credit Card";
                else
                    paymentType = "Money";

                var transaction = new Transaction
                {
                    Description = $"Transaction {transactionNumber}",
                    PaymentType = paymentType,
                    Amount = Convert.ToDouble(transactionNumber*10),
                    TransactionTime = new DateTime(year: 2022, month, day, hour, minute, second)
                };

                transactions.Add(transaction);

                hour++;
                minute++;
                second++;
                day++;

                if (hour == 24)
                {
                    hour = 0;
                }
                if (day > 28)
                {
                    month = 3;
                    day = 1;
                }
            }

            return transactions;
        }

        private static IEnumerable<Account> GenerateAccountEntries()
        {
            List<Account> accounts = new()
            {
                new Account { Id = 1, UserName = "manager", UserPassword = "Boss123", UserRole = "Manager" },
                new Account { Id = 2, UserName = "employee", UserPassword = "Employee123", UserRole = "Employee" },
            };

            return accounts;
        }

        private static IEnumerable<Role> GenerateRoleEntries()
        {
            List<Role> roles = new()
            {
                new Role { RoleName = "Manager", MenuAccessPermissions = "Home;Add Transaction;Transaction List", CommandAccessPermissions = "" },
                new Role { RoleName = "Employee", MenuAccessPermissions = "Home;Add Transaction;Transaction List", CommandAccessPermissions = "" },
            };

            return roles;
        }
    }
}
