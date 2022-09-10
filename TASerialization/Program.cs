using System;
using System.IO;

namespace TASerialization
{
    class Program
    {
        private const string _SerializationFileName = "Serialized Data";

        public static void Test_ListRandIsEmpty()
        {
            ListRand listRand = new ListRand();

            _SerializeList(listRand);
        }

        public static void Test_ListRandContains1Item()
        {
            ListRand listRand = new ListRand();

            ListNode head = new ListNode("head");

            head.Rand = head;

            listRand.Add(head);

            _SerializeList(listRand);
        }

        public static void Test_ListRandFromPicture()
        {
            ListRand listRand = new ListRand();

            ListNode head = new ListNode("head");
            ListNode item1 = new ListNode("item1");
            ListNode item2 = new ListNode("item2");
            ListNode item3 = new ListNode("item3");
            ListNode tail = new ListNode("tail");

            item1.Rand = item1;
            item2.Rand = tail;
            item3.Rand = head;

            listRand.Add(head);
            listRand.Add(item1);
            listRand.Add(item2);
            listRand.Add(item3);
            listRand.Add(tail);

            _SerializeList(listRand);
        }

        private static void _SerializeList(ListRand listRand)
        {
            Console.WriteLine("Before serialization");
            listRand.Show();

            string path = Path.Combine(Environment.CurrentDirectory, _SerializationFileName);

            FileStream writeFileStream = new FileStream(path, FileMode.OpenOrCreate);
            listRand.Serialize(writeFileStream);
            writeFileStream.Close();

            Console.WriteLine("After serialization");
            ListRand secondListRand = new ListRand();

            FileStream readFileStream = File.OpenRead(path);
            secondListRand.Deserialize(readFileStream);
            readFileStream.Close();

            secondListRand.Show();
        }

        static void Main(string[] args)
        {
            Test_ListRandIsEmpty();
            Test_ListRandContains1Item();
            Test_ListRandFromPicture();
        }
    }
}