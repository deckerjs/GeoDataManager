using MongoDB.Bson.Serialization;
using System;
using Newtonsoft.Json;
using GeoDataModels.Models;

namespace GeoStoreAPI.Extensions
{
    //, IBsonDocumentSerializer
    //, IBsonIdProvider
    internal class CustomGeoJsonSerializer : IBsonSerializer
    {
        public Type ValueType { get { return typeof(FeatureCollection); } }

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

        public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var item = JsonConvert.DeserializeObject<FeatureCollection>(context.Reader.ReadString());
            return item;
        }


        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            var item = JsonConvert.SerializeObject(value);
            context.Writer.WriteString(item);
        }


    }
}