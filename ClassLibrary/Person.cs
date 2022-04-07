using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public class Person
    {
        public string Name;
        public string Surname;
        public long ChatId;
        public int Point;

        public Person(string name, string surname, long chatId, int point)
        {
            Name = name;
            Surname = surname;
            ChatId = chatId;
            Point = point;
        }
    }
}
