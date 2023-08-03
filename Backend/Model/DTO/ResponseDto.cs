﻿namespace Model.DTO
{
    public class ResponseDto<T>
    {
        public int StatusCode { get; set; }
        public bool Succeeded { get; set; }
        public string DisplayMessage { get; set; }
        public T Result { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}