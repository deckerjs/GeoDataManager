using MongoDB.Bson.Serialization;
using System;
using Newtonsoft.Json;
using GeoDataModels.Models;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Serializers;


/// <summary>
/// this was a fail, but it may be good for something later
/// error was always: "ReadBsonType can only be called when State is Type, not when State is Value."
/// this happened when filtering the collection, trying many different things never changed the error. 
/// no idea where it is doing the ReadBsonType, or whether its under my control
/// </summary>

namespace GeoStoreAPI.Extensions
{
    //, IBsonDocumentSerializer
    //, IBsonIdProvider
    //, IBsonArraySerializer
    //, IBsonSerializer
    internal class CustomGeoJsonSerializer : SerializerBase<FeatureCollection>
    {
        //public Type ValueType { get { return typeof(FeatureCollection); } }

        //public bool GetDocumentId(object document, out object id, out Type idNominalType, out IIdGenerator idGenerator)
        //{
        //    var item = (GeoData)document;
        //    id = item.ID;
        //}

        //public void SetDocumentId(object document, object id)
        //{
        //    throw new NotImplementedException();
        //}

        //public bool TryGetMemberSerializationInfo(string memberName, out BsonSerializationInfo serializationInfo)
        //{
        //    throw new NotImplementedException();
        //}

        public override FeatureCollection Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            //var item = JsonConvert.DeserializeObject<FeatureCollection>(context.Reader.ReadString());

            //if(context.Reader.State != BsonReaderState.Type || context.Reader.ReadBsonType() != BsonType.EndOfDocument)
            //{
            //    throw new NotImplementedException();
            //}

            if (context.Reader.State == MongoDB.Bson.IO.BsonReaderState.Value && context.Reader.CurrentBsonType == MongoDB.Bson.BsonType.Document)
            {
                var item = Newtonsoft.Json.JsonConvert.DeserializeObject<FeatureCollection>(context.ToJson());
                return item;
            }
            return null;
            //if (context.Reader.CurrentBsonType == MongoDB.Bson.BsonType.Null)
            //{
            //    context.Reader.ReadNull();
            //    return null;
            //}
            //var item = Newtonsoft.Json.JsonConvert.DeserializeObject<FeatureCollection>(context.ToJson());
            //return item;
        }


        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, FeatureCollection value)
        {
            var item = Newtonsoft.Json.JsonConvert.SerializeObject(value);
            context.Writer.WriteString(item);
        }


    }
}