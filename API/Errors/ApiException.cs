using System;

namespace API.Errors;

public class ApiException(int stateCode,string message,string? details)
{
  public int StateCode { get; set; } = stateCode;
  public string Message { get; set; }= message;
  public string? Details { get; set; }= details;
}
