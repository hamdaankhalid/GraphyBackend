using System;
using System.Collections.Generic;
using MongoDB.Driver;
using GraphyBackend.Models;
using MongoDB.Bson;

namespace GraphyBackend.Repositories
{

    public class MongoDbItemsRepository : IItemsRepository
    {
		
		private const string databaseName = "graphyDb";
		private const string collectionName = "items";
		private readonly IMongoCollection<Item> itemsCollection;

        public MongoDbItemsRepository(IMongoClient mongoClient)
        {
				IMongoDatabase database = mongoClient.GetDatabase(databaseName);
				itemsCollection = database.GetCollection<Item>(collectionName);
        }

        public IEnumerable<Item> GetItems()
        {
			return itemsCollection.Find(new BsonDocument()).ToList();
		}

        public Item GetItem(Guid id)
        {
			return itemsCollection.Find()
        }

        public void CreateItem(Item item)
        {
			itemsCollection.InsertOne(item);
        }

        public void UpdateItem(Item item)
        {
			throw new NotImplementedException();
        }

        public void DeleteItem(Guid id)
        {
			throw new NotImplementedException();
        }
	}
}
