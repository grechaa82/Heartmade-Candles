﻿using CSharpFunctionalExtensions;
using HeartmadeCandles.Bot.Core;
using HeartmadeCandles.Bot.Core.Models;
using HeartmadeCandles.Bot.DAL.Documents;
using MongoDB.Driver;

namespace HeartmadeCandles.Bot.DAL;

public class TelegramBotRepository : ITelegramBotRepository
{
    private readonly IMongoCollection<TelegramUserDocument> _telegramUserCollection;

    public TelegramBotRepository(IMongoDatabase mongoDatabase)
    {
        _telegramUserCollection = mongoDatabase
            .GetCollection<TelegramUserDocument>(TelegramUserDocument.DocumentName);
    }

    public async Task<Maybe<TelegramUser>> GetTelegramUserByChatId(long chatId)
    {
        var telegramUserDocument = await _telegramUserCollection
            .Find(x => x.ChatId == chatId)
            .FirstOrDefaultAsync();

        if (telegramUserDocument == null)
        {
            return Maybe.None;
        }

        var telegramUser = new TelegramUser(
            userId: telegramUserDocument.UserId,
            chatId: telegramUserDocument.ChatId,
            userName: telegramUserDocument.UserName,
            firstName: telegramUserDocument.FirstName,
            lastName: telegramUserDocument.LastName,
            currentOrderId: telegramUserDocument.CurrentOrderId,
            state: telegramUserDocument.State,
            role: telegramUserDocument.Role,
            id: telegramUserDocument.Id);

        return telegramUser;
    }

    public async Task<Result<string>> CreateTelegramUser(TelegramUser telegramUser)
    {
        var telegramUserDocument = new TelegramUserDocument()
        {
            Id = telegramUser.Id,
            UserId = telegramUser.UserId, 
            ChatId = telegramUser.ChatId, 
            UserName = telegramUser.UserName, 
            FirstName = telegramUser.FirstName, 
            LastName = telegramUser.LastName, 
            CurrentOrderId = telegramUser.CurrentOrderId, 
            State = telegramUser.State, 
            Role = telegramUser.Role, 
        };

        await _telegramUserCollection.InsertOneAsync(telegramUserDocument);

        return Result.Success(telegramUserDocument.Id);
    }

    public async Task<Result> UpdateOrderId(long chatId, string orderId)
    {
        var update = Builders<TelegramUserDocument>.Update
           .Set(x => x.CurrentOrderId, orderId);

        await _telegramUserCollection.UpdateOneAsync(x => x.ChatId == chatId, update: update);

        return Result.Success();
    }

    public async Task<Result> UpdateTelegramUserState(long chatId, TelegramUserState state)
    {
        var update = Builders<TelegramUserDocument>.Update
           .Set(x => x.State, state);

        await _telegramUserCollection.UpdateOneAsync(x => x.ChatId == chatId, update: update);

        return Result.Success();
    }
}
