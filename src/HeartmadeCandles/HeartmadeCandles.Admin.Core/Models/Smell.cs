﻿namespace HeartmadeCandles.Admin.Core.Models
{
    public class Smell
    {
        private const int MaxTitleLenght = 48;
        private const int MaxDescriptionLenght = 256;
        
        private int _id;
        private string _title;
        private string _description;
        private decimal _price;
        private bool _isActive;

        private Smell(
            int id,
            string title, 
            string description, 
            decimal price, 
            bool isActive)
        {
            _id = id;
            _title = title;
            _description = description;
            _price = price;
            _isActive = isActive;
        }

        public int Id { get => _id; }
        public string Title { get => _title; }
        public string Description { get => _description; }
        public decimal Price { get => _price; }
        public bool IsActive { get => _isActive; }
        
        public static Smell Create(
            string title, 
            string description, 
            decimal price, 
            bool isActive,
            int id = 0)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentNullException($"'{nameof(title)}' connot be null or whitespace.");
            }

            if (title.Length > MaxTitleLenght)
            {
                throw new ArgumentOutOfRangeException($"'{nameof(title)}' connot be more than 48 characters.");
            }

            if (description.Length > MaxDescriptionLenght)
            {
                throw new ArgumentOutOfRangeException($"'{nameof(description)}' connot be more than 256 characters.");
            }

            if (price <= 0)
            {
                throw new ArgumentOutOfRangeException($"'{nameof(price)}' сannot be 0 or less.");
            }
            
            return new Smell(
                id, 
                title, 
                description, 
                price, 
                isActive);
        }
    }
}
