using System;
using System.Text;

namespace RingBufferExercise
{
    public class RingBuffer<T>
    {
        private int readIndex;
        private int writeIndex;
        private bool isDataAvailable;
        private readonly T?[] buffer;

        public RingBuffer(int size)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size), $"Argument '{nameof(size)}' must be strictly positive.");

            buffer = new T[size];
        }

        private int GetNextIndex(int index)
        {
            return (index + 1) % buffer.Length;
        }

        public void Write(T? item)
        {
            if (isDataAvailable && readIndex == writeIndex)
                readIndex = GetNextIndex(readIndex);

            buffer[writeIndex] = item;

            writeIndex = GetNextIndex(writeIndex);

            isDataAvailable = true;
        }

        public bool TryRead(out T? result)
        {
            if (isDataAvailable == false)
            {
                result = default;
                return false;
            }

            result = buffer[readIndex];

            readIndex = GetNextIndex(readIndex);

            if (readIndex == writeIndex)
                isDataAvailable = false;

            return true;
        }

        #region Print methods

        public override string ToString()
        {
            return ToString(true);
        }

        public string ToString(bool showIndexes)
        {
            var sb = new StringBuilder();

            int largestItemWidth = ComputeLargestItemWidth();

            AppendPointerLine(sb, largestItemWidth, "R", readIndex);
            if (showIndexes)
            {
                AppendIndexesLine(sb, largestItemWidth);
                AppendSeparatorLine(sb, largestItemWidth);
            }
            AppendValuesLine(sb, largestItemWidth);
            AppendPointerLine(sb, largestItemWidth, "W", writeIndex);

            return sb.ToString();
        }

        private static string GetString(T? item)
        {
            string str = "null";

            if (item is not null)
            {
                string? temp = item.ToString();
                if (temp is not null)
                    str = temp;
            }

            return str;
        }

        private int ComputeLargestItemWidth()
        {
            int largest = 0;

            for (int i = 0; i < buffer.Length; i++)
            {
                string str = i.ToString();
                largest = Math.Max(largest, str.Length);
            }

            for (int i = 0; i < buffer.Length; i++)
            {
                string str = GetString(buffer[i]);
                largest = Math.Max(largest, str.Length);
            }

            return largest;
        }

        private void AppendPointerLine(StringBuilder sb, int itemWidth, string text, int index)
        {
            sb.Append("  ");
            for (int i = 0; i < buffer.Length; i++)
            {
                if (i == index)
                {
                    sb.Append(text.PadRight(itemWidth, ' '));
                    break;
                }
                else
                    sb.Append(string.Empty.PadRight(itemWidth, ' '));
                sb.Append("   ");
            }
            sb.AppendLine();
        }

        private void AppendIndexesLine(StringBuilder sb, int itemWidth)
        {
            sb.Append("| ");
            for (int i = 0; i < buffer.Length; i++)
            {
                sb.Append(i.ToString().PadRight(itemWidth, ' '));
                sb.Append(" | ");
            }
            sb.AppendLine();
        }

        private void AppendValuesLine(StringBuilder sb, int itemWidth)
        {
            sb.Append("| ");
            for (int i = 0; i < buffer.Length; i++)
            {
                sb.Append(GetString(buffer[i]).PadRight(itemWidth, ' '));
                sb.Append(" | ");
            }
            sb.AppendLine();
        }

        private void AppendSeparatorLine(StringBuilder sb, int itemWidth)
        {
            int size =
                1 + // Heading pipe character.
                (
                    (itemWidth + 3) * // 3 for spaces and pipe separator characters.
                    buffer.Length
                );

            sb.AppendLine(new string('-', size));
        }

        #endregion
    }
}
