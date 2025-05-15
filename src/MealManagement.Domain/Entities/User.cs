using MealManagement.Domain.Enums;
using System;
using System.Collections.Generic;

namespace MealManagement.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal Balance { get; set; }
        public bool IsActive { get; set; }
        public UserRole Role { get; set; }
        public string FirebaseToken { get; set; }
        
        public virtual ICollection<Meal> Meals { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<Transaction> AdjustedTransactions { get; set; }
        
        public User()
        {
            Meals = new HashSet<Meal>();
            Transactions = new HashSet<Transaction>();
            AdjustedTransactions = new HashSet<Transaction>();
        }
    }
}