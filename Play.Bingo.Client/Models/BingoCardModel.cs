﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Play.Bingo.Client.Models
{
    /// <summary> Represents 24 numbers separated by the 5 BINGO columns. </summary>
    public class BingoCardModel
    {
        /// <summary> Constructs a new Bingo card. </summary>
        public BingoCardModel()
        {
        }

        /// <summary> Constructs a Bingo card by exiting numbers. </summary>
        public BingoCardModel(IEnumerable<byte> serializedNumbers)
        {
            var numbers = new List<int>();
            foreach (var numberPair in serializedNumbers)
            {
                var first = Convert.ToInt32(numberPair >> 4);
                var second = Convert.ToInt32(numberPair & 0x0F);
                numbers.AddRange(new[] {first, second});
            }

            B = Pick(numbers, 0, 0);
            I = Pick(numbers, 5, 15);
            N = Pick(numbers, 10, 30, 4);
            N = new[] {N[0], N[1], 0, N[2], N[3]};
            G = Pick(numbers, 14, 45);
            O = Pick(numbers, 19, 60);
        }

        #region Bingo numbers.

        public int[] B { get; set; }
        public int[] I { get; set; }
        public int[] N { get; set; }
        public int[] G { get; set; }
        public int[] O { get; set; }

        #endregion

        public byte[] ToBinary()
        {
            var numbers = new List<int>(25);
            numbers.AddRange(B);
            numbers.AddRange(I.Select(i => i - 15));
            numbers.AddRange(N.Except(new[] {0}).Select(n => n - 30));
            numbers.AddRange(G.Select(g => g - 45));
            numbers.AddRange(O.Select(o => o - 60));

            var binary = new List<byte>();
            for (var index = 0; index < numbers.Count; index += 2)
            {
                var number1 = numbers[index];
                var number2 = numbers[index + 1];
                binary.Add(Convert.ToByte(number1 << 4 | number2));
            }
            return binary.ToArray();
        }

        #region Private helper methods.

        private static int[] Pick(IEnumerable<int> numbers, int skip, int offset, int take = 5)
        {
            return numbers
                .Skip(skip)
                .Take(take)
                .Select(n => n + offset)
                .ToArray();
        }

        #endregion
    }
}