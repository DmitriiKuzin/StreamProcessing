﻿namespace DAL;

public class Account
{
    public long Id { get; set; }
    public string CreditCard { get; set; }
}

public class AccountTransaction
{
    public long Id { get; set; }
    public DateTime DateTime { get; set; }
    public double Amount { get; set; }

    public long AccountId { get; set; }
    public Account Account { get; set; }
}