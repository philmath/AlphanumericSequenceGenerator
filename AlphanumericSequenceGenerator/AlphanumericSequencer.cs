using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AlphanumericSequenceGenerator
{
    public class AlphanumericSequencer
    {
        private const string LOW_ALPHA = "lowAlpha";
        private const string UP_ALPHA = "upAlpha";
        private const string NUMERIC = "numeric";

        private readonly List<BoundPart> _lowerBoundParts;
        private readonly List<BoundPart> _upperBoundParts;
        private readonly List<BoundPart> _boundSeparators;

        public AlphanumericSequencer(string lowerBound, string upperBound)
        {
            // Get bound parts
            _lowerBoundParts = GetBoundAlphanumericParts(lowerBound);
            _upperBoundParts = GetBoundAlphanumericParts(upperBound);

            // Check bounds coherence
            CheckBoundAlphanumericParts(_lowerBoundParts, _upperBoundParts);

            // Get separator parts
            _boundSeparators = GetBoundSeparators(lowerBound);
            var upperBoundSeparators = GetBoundSeparators(upperBound);

            // Check separators coherence
            CheckBoundSeparators(_boundSeparators, upperBoundSeparators);
        }

        private List<BoundPart> GetBoundAlphanumericParts(string bound)
        {
            var alphanumGroupsRegex = new Regex($"(?<{LOW_ALPHA}>[a-z]+)|(?<{UP_ALPHA}>[A-Z]+)|(?<{NUMERIC}>\\d+)", RegexOptions.Compiled);
            var matches = alphanumGroupsRegex.Matches(bound);
            var boundParts = new List<BoundPart>();

            foreach (Match match in matches)
            {
                foreach (Group group in match.Groups)
                {
                    if (group.Success && group.Name != "0")
                    {
                        boundParts.Add(new BoundPart(group.Value, group.Index, group.Length, group.Name));
                    }
                }
            }

            return boundParts;
        }

        private void CheckBoundAlphanumericParts(List<BoundPart> lowers, List<BoundPart> uppers)
        {
            if (lowers.Count != uppers.Count)
                throw new Exception($"The number of groups in bounds are different: {lowers.Count} / {uppers.Count}.");

            for (var i = 0; i < lowers.Count; i++)
            {
                if (lowers[i].groupType != uppers[i].groupType)
                    throw new Exception($"Group types in bounds are different: {lowers[i].part} / {uppers[i].part}.");

                if (lowers[i].length > uppers[i].length)
                    throw new Exception($"A group length in the lower bound is longer than in the upper bound: {lowers[i].part} > {uppers[i].part}.");

                var lower = lowers[i].part;
                var upper = uppers[i].part;
                if (lower.Length < upper.Length)
                    lower = new string(' ', upper.Length - lower.Length) + lower;
                if (lower.CompareTo(upper) > 0)
                    throw new Exception($"A group in the lower bound follows the related group in the upper bound: {lowers[i].part} > {uppers[i].part}.");
            }
        }

        private List<BoundPart> GetBoundSeparators(string bound)
        {
            var separatorsRegex = new Regex(@"(\W|_)+", RegexOptions.Compiled);
            var matches = separatorsRegex.Matches(bound);
            var separators = new List<BoundPart>();

            foreach (Match match in matches)
            {
                separators.Add(new BoundPart(match.Value, match.Index, match.Length, null));
            }

            return separators;
        }

        private void CheckBoundSeparators(List<BoundPart> lowerSeparators, List<BoundPart> upperSeparators)
        {
            if (lowerSeparators.Count != upperSeparators.Count)
                throw new Exception($"The number of separators in bounds are different: {lowerSeparators.Count} / {upperSeparators.Count}.");

            for (var i = 0; i < lowerSeparators.Count; i++)
            {
                if (lowerSeparators[i].part != upperSeparators[i].part)
                    throw new Exception($"Separators in bounds are different: {lowerSeparators[i].part} / {upperSeparators[i].part}.");
            }
        }

        public IEnumerable<string> GetSequence() => GetSequence(_lowerBoundParts, _upperBoundParts, _boundSeparators);

        private IEnumerable<string> GetSequence(List<BoundPart> lowers, List<BoundPart> uppers, List<BoundPart> separators, string prefix = "")
        {
            var fullSequence = new List<string>();
            IEnumerable<string> sequence = null;

            // Get the sequence for the first bound pair
            sequence = GetSubsequence(lowers[0], uppers[0], prefix);

            var nextLowers = lowers.GetRange(1, lowers.Count - 1);
            var nextUppers = uppers.GetRange(1, uppers.Count - 1);

            if (nextLowers.Count > 0)
            {
                var separator = "";
                var nextSeparators = separators;
                if (separators.Count > 0 && nextSeparators[0].index < nextLowers[0].index)
                {
                    separator = nextSeparators[0].part;
                    nextSeparators = separators.GetRange(1, separators.Count - 1);
                }

                // Get the sequences for the next bound pairs
                foreach (var item in sequence)
                {
                    fullSequence.AddRange(GetSequence(nextLowers, nextUppers, nextSeparators, item + separator));
                }
            }
            else
                fullSequence.AddRange(sequence);

            return fullSequence;
        }

        private IEnumerable<string> GetSubsequence(BoundPart lower, BoundPart upper, string prefix)
        {
            var chars = new StringBuilder(lower.part);

            yield return prefix + chars.ToString();

            char start = ' ', end = ' ', next = ' ';
            switch (lower.groupType)
            {
                case LOW_ALPHA:
                    start = 'a';
                    end = 'z';
                    next = start;
                    break;

                case UP_ALPHA:
                    start = 'A';
                    end = 'Z';
                    next = start;
                    break;

                case NUMERIC:
                    start = '0';
                    end = '9';
                    next = '1';
                    break;
            }

            // Generate the sequence
            do
            {
                int i = chars.Length - 1;
                while (i >= 0 && chars[i] == end)
                {
                    chars[i] = start;
                    i--;
                }
                if (i == -1)
                    chars.Insert(0, next);
                else
                    chars[i]++;
                yield return prefix + chars.ToString();
            }
            while (chars.ToString() != upper.part);
        }
    }
}
