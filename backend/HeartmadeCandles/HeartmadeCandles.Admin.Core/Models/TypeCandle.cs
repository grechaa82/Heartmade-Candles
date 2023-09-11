﻿using CSharpFunctionalExtensions;

namespace HeartmadeCandles.Admin.Core.Models
{
    public class TypeCandle
    {
        public const int MaxTitleLenght = 32;

        private int _id;
        private string _title;

        private TypeCandle(int id, string title)
        {
            _id = id;
            _title = title;
        }

        public int Id { get => _id; }
        public string Title { get => _title; }

        public static Result<TypeCandle> Create(string title, int id = 0)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return Result.Failure<TypeCandle>($"'{nameof(title)}' cannot be null or whitespace");
            }

            if (!string.IsNullOrWhiteSpace(title) && title.Length > MaxTitleLenght)
            {
                return Result.Failure<TypeCandle>($"'{nameof(title)}' cannot be more than {MaxTitleLenght} characters");
            }

            var typeCandle = new TypeCandle(id, title);

            return Result.Success(typeCandle);
        }
    }
}