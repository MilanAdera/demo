using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;

namespace Grains
{
    public interface IStateHolderGrain<T> : IGrainWithGuidKey
    {
        Task<T> GetItem();
        Task<T> SetItem(T obj);
        Task<List<T>> GetItems();
    }

    public class StateHolder<T>
    {
        public StateHolder() : this(default(T))
        {
            this.Values = new List<T>();
        }

        public StateHolder(T value)
        {
            Value = value;
            if (Values == null)
            {
                Values = new List<T>();
            }
            Values.Add(value);
        }

        public StateHolder(List<T> values)
        {
            Values = values;
        }

        public T Value { get; set; }
        public List<T> Values { get; set; }
    }

    public abstract class StateHolderGrain<T> : Grain<StateHolder<T>>,
        IStateHolderGrain<T>
    {
        public Task<T> GetItem()
        {
            return Task.FromResult(State.Value);
        }

        public async Task<T> SetItem(T item)
        {
            State.Value = item;
            if (State.Values == null)
            {
                State.Values = new List<T>();
            }
            State.Values.Add(item);
            await WriteStateAsync();

            return State.Value;
        }

        public async Task<List<T>> GetItems()
        {
            //return State.Values;
            return await Task.FromResult(State.Values);
        }
    }
}
