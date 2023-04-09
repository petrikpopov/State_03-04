using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _03._04._23_HW1
{
    public class Account
    {
        private State state;
        private string Owner;

        public Account(string owner)
        {
            Owner = owner;
        }

        public State GetState()
        {
            return state;
        }

        public void SetState(State state)
        {
            this.state = state;
            state.SetAccount(this);
        }

        public double GetBalance()
        {
            return state.GetBalance();
        }
        

        public void Deposit(double amount)
        {
            state.Deposit(amount);

            Console.WriteLine($"Deposited:{amount}$");

            Console.WriteLine($"Balance:{GetBalance()}$");

            Console.WriteLine($"Status:{GetState().GetType().Name}");

        }
        public void PayInterest()
        {
            bool Result_2 = state.PayInterest();

            if (Result_2)
            {
                Console.WriteLine($"Balance:{GetBalance()}$");

                Console.WriteLine($"Status:{GetState().GetType().Name}");
               
            }
        }

        public void Withdraw(double Amount)
        {
            bool Result_1 = state.Withdraw(Amount);

            if (Result_1)
            {
                Console.WriteLine($"Withdraw:{Amount}$");
                Console.WriteLine($"Balance:{GetBalance()}$");
                Console.WriteLine($"Status:{GetState().GetType().Name}");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Не удалось провести транзакцию");
            }
        }
    }
    public abstract class State
    {
        public Account account;
        public double balance;
        public double interest;
        public double lowerLimit;
        public double upperLimit;

        public Account GetAccount()
        {
            return account;
        }

        public void SetAccount(Account account)
        {
            this.account = account;
        }

        public double GetBalance()
        {
            return balance;
        }

        public void SetBalance(double balance)
        {
            this.balance = balance;
        }

        public abstract void Deposit(double amount);

        public abstract bool Withdraw(double amount);

        public abstract bool PayInterest();
    }
    public class SilverState : State
    {
        public SilverState(double balance, Account account)
        {
            this.balance = balance;
            this.account = account;
            Initialize();
        }
        public SilverState(State state) : this(state.GetBalance(), state.GetAccount())
        {
            Initialize();
            StateChangeCheck();
        }

        private void Initialize()
        {
            interest = 0.1;
            lowerLimit = 0.0;
            upperLimit = 10000.0;
        }

        private void StateChangeCheck()
        {
            if (balance < lowerLimit)
            {
                account.SetState(new RedState(this));
            }
            else if (balance >= upperLimit)
            {
                account.SetState(new GoldState(this));
            }
        }

        public override void Deposit(double amount)
        {
            balance += amount;
            StateChangeCheck();
        }

        public override bool Withdraw(double amount)
        {
            balance -= amount;
            StateChangeCheck();
            return true;
        }
        public override bool PayInterest()
        {
            balance += interest * balance;
            StateChangeCheck();
            return true;
        }
    }
    public class RedState : State
    {
        public RedState(double balance, Account account)
        {
            this.balance = balance;
            this.account = account;
            Initialize();
        }
        public RedState(State state) : this(state.GetBalance(), state.GetAccount())
        {
            Initialize();
            StateChangeCheck();
        }
        private void Initialize()
        {
            interest = 0.0;
            lowerLimit = -50.0;
            upperLimit = 0.0;
        }

        private void StateChangeCheck()
        {
            if (balance >= lowerLimit && balance < upperLimit)
            {
                account.SetState(new SilverState(this));
            }
            else if (balance >= upperLimit)
            {
                account.SetState(new GoldState(this));
            }
        }

        public override void Deposit(double amount)
        {
            balance += amount;
            StateChangeCheck();
        }

        public override bool Withdraw(double amount)
        {
            Console.WriteLine("No funds available for withdrawal!\n");
            return false;
        }

        public override bool PayInterest()
        {
            Console.WriteLine("No interest is paid!\n");
            return false;
        }
    }
    public class GoldState : State
    {
        public GoldState(double balance, Account account)
        {
            this.balance = balance;
            this.account = account;
            Initialize();
        }
        public GoldState(State state) : this(state.GetBalance(), state.GetAccount())
        {
            Initialize();
            StateChangeCheck();
        }

        private void Initialize()
        {
            interest = 0.10;
            lowerLimit = 10000.0;
            upperLimit = 1100000.0;
        }
        private void StateChangeCheck()
        {
            if (balance < lowerLimit)
            {
                account.SetState(new RedState(this));
            }
            else if (balance >= upperLimit)
            {
                account.SetState(new SilverState(this));
            }
        }
        public override void Deposit(double amount)
        {
            balance += amount;
            StateChangeCheck();
        }

        public override bool Withdraw(double amount)
        {
            if (balance - amount < lowerLimit)
            {
                return false;
            }
            balance -= amount;
            StateChangeCheck();
            return true;
        }

        public override bool PayInterest()
        {
            balance += interest * balance;
            StateChangeCheck();
            return true;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Account account = new Account("Bank depos");
            account.SetState(new SilverState(0.0,account));
            account.Deposit(5000.20);
            account.Deposit(4000.10);
            account.PayInterest();
            account.Deposit(4300.0);
            account.PayInterest();
            account.Withdraw(2000.0);
            account.Withdraw(1570.0);
            account.PayInterest();

        }
    }
}