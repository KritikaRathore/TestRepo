using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AderantCodingTestSolution
{
    class GreedyMatchMerge
    {
        private readonly Dictionary<int, string> fragDic = new Dictionary<int, string>();
        private int dicKeyCount;
        //Constructor which intializes the Input 
        public GreedyMatchMerge(Dictionary<int, string> fileString, int count)
        {
            this.fragDic = fileString;
            this.dicKeyCount = count;
        }
        //Returns final String formed
        public string GetFinalOutput()
        {
            var finalString = "";
            if (this.dicKeyCount == 2)
            {
                finalString = this.fragDic[1];
            }
            else
            {
                finalString = RunAlgo();
            }
            return finalString;
        }
        /* private method which runs the greedy approach of matching the string fragments based on max overlap 
           and then merge those fragments , creating the one possible outcome*/
        private string RunAlgo()
        {
            var finalString = "";
            bool runCount = false;
            int s1 = 0, s2 = 0;        // s1,s2 stores the Key of string1 and string2 which will merge
            List<Combo> comboList = new List<Combo>();
            //loop through the ddictionary until only one Key remains
            while (this.fragDic.Count > 1)
            {
                // create all possible combinations of the fragments and discover their overlap string and the overlap rank i.e
                // the max value of the overlapping characters
                if (runCount == false)
                {
                    // create all combinations for original fragments
                    comboList = CreateAllCombos(this.fragDic, this.dicKeyCount);
                    runCount = true;
                }
                else
                {
                    // update the combo list to only contain the fragment combinations after the merge and update of fragment dictionary
                    comboList = UpdateComboList(this.fragDic, comboList, this.dicKeyCount, s1, s2);
                }
                if (comboList.Any())
                {
                    //Find the max overlap combo in the list
                    var maxRank = comboList.Where(o => o.OverlapRank == comboList.Max(e => e.OverlapRank)).First();
                    //Check for Max value
                    if (maxRank.OverlapRank == 0)   // when all fragments remaining doesn't have any overlap
                    {
                        foreach (var pair in this.fragDic)
                        {
                            finalString += pair.Value;  // merge all the remaining fragments
                        }
                        //Empty the dictionary
                        this.fragDic.Clear();
                    }
                    else                             // when max overlap found
                    {
                        //Remove the entry for the fragments which had max overlap from original dictionary
                        this.fragDic.Remove(maxRank.Str1);
                        this.fragDic.Remove(maxRank.Str2);
                        //Add the new fragment overlapped string to dictionary post a max match found
                        this.fragDic.Add(this.dicKeyCount++, maxRank.OverlapStr);
                        //Store the Key of merged string fragments to update the combo list
                        s1 = maxRank.Str1;
                        s2 = maxRank.Str2;
                    }
                }

            }
            return finalString;
        }
        /* This function creates a list of all fragment combos and their max overlap and the overlap string
         * this function runs on original dictionary only 
         * Stores an Id- unique value for combo of fragments, Str1- Key of string 1 , Str2- Key of string 2
         * OverlapStr -Overlap String after match found , OverlapRank - Stores max overlap value
           Returns : A list of combinations*/
        private List<Combo> CreateAllCombos(Dictionary<int, string> allStr, int len)
        {
            var combo = new List<Combo>();
            int count = 1;
            for (var i = 1; i < len; i++)
            {
                for (var j = i + 1; j < len; j++)
                {
                    string overlapStr = "";
                    //Find overlap for combo s1-s2 i.e end of s1 with start of s2
                    int overlapRank = FindOverlapRankStr(allStr[i], allStr[j], ref overlapStr);
                    combo.Add(new Combo { Id = count++, Str1 = i, Str2 = j, OverlapStr = overlapStr, OverlapRank = overlapRank });
                    overlapStr = "";
                    //Find overlap for combo s2-s1 i.e end of s2 with start of s1
                    overlapRank = FindOverlapRankStr(allStr[j], allStr[i], ref overlapStr);
                    combo.Add(new Combo { Id = count++, Str1 = j, Str2 = i, OverlapStr = overlapStr, OverlapRank = overlapRank });

                }
            }
            return combo;
        }
        /*This function updates the combo List by below functions:
         * 1.Remove the combos which included the max overlap strings s1,s2 as they merged into new fragment
         * 2.Update the combo list with new fragment combos with remaining fragments
         */
        private List<Combo> UpdateComboList(Dictionary<int, string> allStr, List<Combo> lastList, int len, int s1, int s2)
        {
            var combo = lastList;
            //list to store the Ids which contains the string1 and string2 which merged to form new fragment
            List<int> cIdList = new List<int>();
            //stores the next possible Id for combo list
            int count = lastList.Max(x => x.Id) + 1;
            foreach (var c in lastList)
            {
                //Check if any combinations have string1 Key or string2 Key
                if (c.Str1 == s1 || c.Str2 == s2 || c.Str1 == s2 || c.Str2 == s1)
                {
                    cIdList.Add(c.Id);
                }
            }
            foreach (var i in cIdList)
            {
                //Remove the entries from the combo list
                combo.RemoveAll(x => x.Id == i);
            }
            //Update the list to add new fragment combinations
            foreach (var pair in allStr)
            {
                if (pair.Key != len - 1)
                {
                    string overlapStr = "";
                    //Find overlap for combo s1-newString i.e end of s1 with start of newString
                    int overlapRank = FindOverlapRankStr(pair.Value, allStr[len - 1], ref overlapStr);
                    combo.Add(new Combo { Id = count++, Str1 = pair.Key, Str2 = len - 1, OverlapStr = overlapStr, OverlapRank = overlapRank });
                    overlapStr = "";
                    //Find overlap for combo newString-s1 i.e start of s1 with end of newString
                    overlapRank = FindOverlapRankStr(allStr[len - 1], pair.Value, ref overlapStr);
                    combo.Add(new Combo { Id = count++, Str1 = len - 1, Str2 = pair.Key, OverlapStr = overlapStr, OverlapRank = overlapRank });
                }
            }
            return combo;
        }
        /*This function calculates the max overlap for string s1, string s2 
         * and stores the overlapped string in refOverlapStr
         * Returns the OverlapRank i.e the max overlap value for the string 1 and string 2 combo
         */
        private int FindOverlapRankStr(string s1, string s2, ref string refOverlapStr)
        {
            int rank = 0;
            int len1 = s1.Length;
            int len2 = s2.Length;
            for (int i = 1; i <= Math.Min(len1, len2); i++)
            {
                // compare last i characters in s1 with first i characters in s2                
                if (String.Compare(s1, len1 - i, s2, 0, i) == 0)
                {
                    if (rank < i)
                    {
                        //update max and str
                        rank = i;
                        refOverlapStr = s1 + s2.Substring(i);
                    }
                }
            }
            return rank;
        }

    }
    // This class is used for creating all the possible comboinations of the string like s1 s2, s2 s3 etc.
    //Stores an Id- unique value for combo of fragments, Str1- Key of string 1 , Str2- Key of string 2
    //OverlapStr -Overlap String after match found , OverlapRank - Stores max overlap value
    public class Combo
    {
        public int Id { get; set; }
        public int Str1 { get; set; }
        public int Str2 { get; set; }
        public string OverlapStr { get; set; }
        public int OverlapRank { get; set; }
    }
}
