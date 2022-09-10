namespace TASerialization
{
    public class ListNode
    {
        public ListNode Prev;
        public ListNode Next;
        public ListNode Rand;
        public string data;

        public ListNode(string newData)
        {
            Prev = null;
            Next = null;
            Rand = null;
            data = newData;
        }
    }
}