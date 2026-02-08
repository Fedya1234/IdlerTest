using System.Collections.Generic;
using Core.Res;
using Core.Storage;

namespace Core.Converter
{
  public class ConverterState
  {
    public Dictionary<ResId, StorageState> InputStorage = new();
    public Dictionary<ResId, StorageState> OutputStorages = new();
    public float Progress = 0f;
  }
}