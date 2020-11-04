using System;
using System.Net.Sockets;
using Data.IO;
using Data.Serialization;

namespace Logic
{
    public class DataStorage
    {
        private Person[] _storage;
        private int _index = 0;
        
        public DataStorage()
        {
            _storage = new Person[2];
        }

        public void LoadData(DataReader reader, DataProcessor processor)
        {
            AddAll(processor.ProcessRead(reader.ReadData()));
        }

        public void SaveData(DataWriter writer, DataProcessor processor)
        {
            writer.WriteData(processor.ProcessWrite(Serialize()));
        }

        public int FindId(string name)
        {
            for (var i = 0; i < _storage.Length; i++)
            {
                if (_storage[i]!=null && _storage[i].DataName == name) return i;
            }

            return -1;
        }

        public Person FindData(string name)
        {
            var id = FindId(name);
            if (id >= 0) return _storage[id];
            return null;
        }

        public Person[] ToArray()
        {
            Collapse();

            var ret = new Person[_index];
            Array.Copy(_storage,ret,_index);
            return ret;
        }

        public SerializedData[] Serialize()
        {
            var data = ToArray();
            var ret = new SerializedData[data.Length];
            for (var i = 0; i < data.Length; i++)
            {
                ret[i] = data[i].Serialize();
            }

            return ret;
        }

        public void Collapse()
        {
            var step = 0;
            for (int i = _index; i < _storage.Length; i++)
            {
                if(_storage[i]!=null)
                {
                    if (step <= 0) continue;
                    _storage[i - step] = _storage[i];
                    _index = i - step + 1;
                }
                else
                {
                    step++;
                }
            }
        }
        
        public void AddAll(SerializedData[] peopleData)
        {
            AddAll(Processor.DeserializePeople(peopleData));
        }
        public void AddAll(Person[] people)
        {
            for (int i = 0; i < people.Length; i++)
            {
                Add(people[i]);
            }
        }
        public void Add(SerializedData personData)
        {
            Add(Processor.DeserializePerson(personData));
        }
        public void Add(Person p)
        {
            try
            {
                while (_storage[_index] != null)
                {
                    _index++;
                }
            }
            catch (IndexOutOfRangeException)
            {
                var newArray = new Person[_storage.Length*2];
                Array.Copy(_storage,newArray,_storage.Length);
                _storage = newArray;
            }

            _storage[_index++] = p;
        }

        public void ResolveConflicts()
        {
            for (int i = 0; i < _storage.Length; i++)
            {
                if (_storage[i] == null) continue;
                var name = _storage[i].DataName;
                for (int j = _storage.Length - 1; j >= i+1; j--)
                {
                    if (_storage[j]!=null && _storage[j].DataName == name)
                    {
                        _remove(i);
                        break;
                    }
                }
            }
            Collapse();
        }

        public Person Remove(string name)
        {
            var id = FindId(name);
            return _remove(id);
        }

        private Person _remove(int id)
        {
            if (id >= 0)
            {
                var ret = _storage[id];
                _storage[id] = null;
                if(id<_index)_index = id;
                return ret;
            }
            return null;
        }

        public void Clear()
        {
            _storage=new Person[2];
            _index = 0;
        }
    }
}