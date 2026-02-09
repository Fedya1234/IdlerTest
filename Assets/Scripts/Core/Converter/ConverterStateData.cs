using System.Collections.Generic;
using Core.Res;

namespace Core.Converter
{
  public class ConverterStateData
  {
    private readonly ResId _mainOutPutId;
    
    private List<ResId> _fullOutputs;
    private List<ResId> _emptyInputs;
    public List<ResId> FullOutputs => _fullOutputs;
    public List<ResId> EmptyInputs => _emptyInputs;
    public ResId MainOutputId => _mainOutPutId;
    
    public bool IsStopped => FullOutputs.Count > 0 || EmptyInputs.Count > 0;
    
    public ConverterStateData(ResId mainOutPutId, List<ResId> fullOutputs, List<ResId> emptyInputs)
    {
      _mainOutPutId = mainOutPutId;
      _fullOutputs = fullOutputs;
      _emptyInputs = emptyInputs;
    }
  }
}