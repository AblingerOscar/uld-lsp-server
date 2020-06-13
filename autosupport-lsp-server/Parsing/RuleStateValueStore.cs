﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using static autosupport_lsp_server.Parsing.IRuleStateValueStoreKey;

namespace autosupport_lsp_server.Parsing
{
    internal class RuleStateValueStore: IEnumerable<KeyValuePair<IRuleStateValueStoreKey, object>>, IEnumerable
    {
        private readonly IDictionary<IRuleStateValueStoreKey, object> values = new Dictionary<IRuleStateValueStoreKey, object>();

        public RuleStateValueStore() { }

        public RuleStateValueStore(RuleStateValueStore valueStore)
        {
            values = new Dictionary<IRuleStateValueStoreKey, object>(valueStore.values);
        }

        public int Count => values.Count;

        public void Add<T>(RuleStateValueStoreKey<T> key, T value) where T : class
        {
            values.Add(key, value);
        }

        public void Clear()
        {
            values.Clear();
        }

        public bool ContainsKey(IRuleStateValueStoreKey key)
        {
            return values.ContainsKey(key);
        }

        public T Get<T>(RuleStateValueStoreKey<T> key)
        {
            if (!values.TryGetValue(key, out object? value))
                throw new IndexOutOfRangeException();

            if (!(value is T castValue))
                throw new ArgumentException($"Value was not of type {typeof(T)}");

            return castValue;
        }

        public IEnumerator<KeyValuePair<IRuleStateValueStoreKey, object>> GetEnumerator()
        {
            return values.GetEnumerator();
        }

        public bool Remove(IRuleStateValueStoreKey key)
        {
            return values.Remove(key);
        }

        public bool Remove(KeyValuePair<IRuleStateValueStoreKey, object> item)
        {
            return values.Remove(item);
        }

        public bool TryGetValue<T>(RuleStateValueStoreKey<T> key, [MaybeNullWhen(false)] out T value)
        {
            value = default;

            if (!values.TryGetValue(key, out object? obj) || !(obj is T castObj))
                return false;

            value = castObj;
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)values).GetEnumerator();
        }
    }

    internal interface IRuleStateValueStoreKey { }

    internal static class RuleStateValueStoreKey {
#pragma warning disable CS0618 // Type or member is obsolete
        public static readonly RuleStateValueStoreKey<string> NextType = new RuleStateValueStoreKey<string>(0);
#pragma warning restore CS0618 // Type or member is obsolete
    }

    internal struct RuleStateValueStoreKey<T>: IRuleStateValueStoreKey
    {
        private readonly byte id;

        [Obsolete("only use the static methods of " + nameof(RuleStateValueStoreKey))]
        internal RuleStateValueStoreKey(byte id)
        {
            this.id = id;
        }

        public override bool Equals(object? obj) => obj is RuleStateValueStoreKey<T> that && id == that.id;

        public override int GetHashCode() => id;
    }
}
