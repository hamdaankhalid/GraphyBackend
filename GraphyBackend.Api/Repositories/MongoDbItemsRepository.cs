using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphyBackend.Api.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GraphyBackend.Api.Repositories
{

    public class MongoDbItemsRepository : IItemsRepository
    {
		
		private const string databaseName = "graphyDb";
		private const string collectionName = "items";
		private readonly IMongoCollection<Item> itemsCollection;
		private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

        public MongoDbItemsRepository(IMongoClient mongoClient)
        {
				IMongoDatabase database = mongoClient.GetDatabase(databaseName);
				itemsCollection = database.GetCollection<Item>(collectionName);
        }

        public async Task<IEnumerable<Item>> GetItems()
        {
			return (await itemsCollection.FindAsync(new BsonDocument())).ToList();
		}

        public async Task<Item> GetItem(Guid id)
        {
			var filter = filterBuilder.Eq(item => item.Id, id);
			return await itemsCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task CreateItem(Item item)
        {
			await itemsCollection.InsertOneAsync(item);
        }

        public async Task UpdateItem(Item item)
        {
			var filter = filterBuilder.Eq(existingItem => existingItem.Id, item.Id);
			await itemsCollection.FindOneAndReplaceAsync(filter, item);
        }

        public async Task DeleteItem(Guid id)
        {
			var filter = filterBuilder.Eq(item => item.Id, id);
			await itemsCollection.DeleteOneAsync(filter);
		}
	}
}

