namespace ChecklistTracker.CoreUtils
{
    public class CircularQueue<T> where T : notnull
    {
        private T[] _queue;
        private int _index;

        public CircularQueue(IEnumerable<T> data)
        {
            _queue = data.ToArray();
            _index = 0;
        }

        public bool Any() => _queue.Any();

        public T Next()
        {
            var ret = _queue[_index++];
            _index %= _queue.Length;
            return ret;
        }
    }

    public static class CircularQueueExtensions
    {
        public static CircularQueue<T> ToCircularQueue<T>(this IEnumerable<T> me)
        {
            return new CircularQueue<T>(me);
        }
    }
}
