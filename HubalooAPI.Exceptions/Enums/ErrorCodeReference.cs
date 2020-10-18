using HubalooAPI.Exceptions.Attributes;

namespace HubalooAPI.Exceptions.Enums
{
    public enum ErrorCodeReference
    {
        [ErrorCode("1001", "General Unexpected Error.")]
        GeneralUnexpected = 1001
    }
}