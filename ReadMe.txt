This is a C# Console Application which provides solution to Test case for below mentioned problem statement:
Write a program that reads a file of text fragments and attempts to reconstruct the original document out of the fragments.The fragments were created by duplicating the original document many times over and chopping each copy into pieces.The fragments overlap one another and your program will search for overlaps and align the fragments to reassemble them into their original order.
  Input- A file containing text fragments which are line seprated.
  Ouput- A string which is formed post aligining all fragments.
  
  Execute the Appliaction:
  Download the application. Run the application in Visual Studio.

Algorithm Explanation:
  The solution is based on a Greedy Match Merge approach.The steps followed in the program are as follows:
    1. Enter the Input file name.
    2. Store all fragments in a dictionary with a Key value.
      a. Check if no fragments.
      b. Check if current fragment is substring of existing fragment in dictionary then do not store the          current fragment, or
      c. If an existing fragment is substring of current fragment, check the key of existing fragment and          replace the value 
         with larger string.
      d. Else Store the fragment.
    3. Call the GreedyMatchMerge Algo to formulate the final output string.
    4. Run the Algo on fragments stored in dictionary case by case:
      a. Case 1 -When only single fragment, retun the value as final output.
      b. Case 2- Run until count of fragments is more than 1:
        i.   Create all the possible combinations of the fragments and store in list.
        ii.  Calculate the overlap rank i.e the overlap value for each combination and the overlap string after              merge.
        iii. Store the fragment 1 and fragment 2 Key along with overlap rank and overlap string in list.
        iv.  Check the Max Overlap Rank for all combinations and overlap string.
        v.   Add new fragment with overlap string and remove the fragments which were merged.
        vi.  Update the combination list by removing the combos containing fragments which were merged and              adding new combinations for new fragment and any other fragment in dictionary.
        vii. Repeat Step ii to vi until Max OverlapRank is 0, then just concat all the fragments as no overlap              left and return the final output.
        
In step iv to calculate Overlap Rank and Overlap String, calculate the overlap by checking the match of characters from end of string1 and start of string2. The number of characters that match is the rank and the overlap string is the string that merges till the character match found.