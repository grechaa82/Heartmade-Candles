﻿using CSharpFunctionalExtensions;

namespace HeartmadeCandles.Admin.Core.Models
{
    public class Wick
    {
        public const int MaxTitleLenght = 48;
        public const int MaxDescriptionLenght = 256;
        
        private int _id;
        private string _title;
        private string _description;
        private decimal _price;
        private string _imageURL;
        private bool _isActive;

        private Wick( 
            int id,
            string title, 
            string description, 
            decimal price, 
            string imageURL, 
            bool isActive)
        {
            _id = id;
            _title = title;
            _description = description;
            _price = price;
            _imageURL = imageURL;
            _isActive = isActive;
        }

        public int Id { get => _id; }
        public string Title { get => _title; }
        public string Description { get => _description; }
        public decimal Price { get => _price; }
        public string ImageURL { get => _imageURL; }
        public bool IsActive { get => _isActive; }
        
        public static Result<Wick> Create(
            string title,
            string description,
            decimal price,
            string imageURL,
            bool isActive,
            int id = 0)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return Result.Failure<Wick>($"'{nameof(title)}' connot be null or whitespace.");
            }

            if (title.Length > MaxTitleLenght)
            {
                return Result.Failure<Wick>($"'{nameof(title)}' connot be more than {MaxTitleLenght} characters.");
            }

            if (description == null)
            {
                return Result.Failure<Wick>($"'{nameof(description)}' connot be null.");
            }

            if (description.Length > MaxDescriptionLenght)
            {
                return Result.Failure<Wick>($"'{nameof(description)}' connot be more than {MaxDescriptionLenght} characters.");
            }

            if (price <= 0)
            {
                return Result.Failure<Wick>($"'{nameof(price)}' сannot be 0 or less.");
            }
            
            var wick =  new Wick(
                id, 
                title, 
                description, 
                price, 
                imageURL, 
                isActive);

            return Result.Success(wick);
        }
    }
}