using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Key를 가지는 데이터 클래스의 인터페이스.
/// </summary>
public interface IDataKey
{
    int Key { get; set; }

    public string ToString();
}