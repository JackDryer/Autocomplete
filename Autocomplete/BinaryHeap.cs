using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autocomplete
{
    internal class BinaryHeap<T>
    {
        List<double> arr;
        List<T> arr2;
        int sizeOfTree;
        public int Count { get => sizeOfTree; }
        //public int Count { get => arr.Count-1; }
        // Create a constructor  
        public BinaryHeap(int size,T none)
        {
            //We are adding size+1, because array index 0 will be blank.  
            arr = new List<double>(size + 1);
            arr2 = new List<T>(size+1);
            arr.Add(0);//never used
            arr2.Add(none);
            sizeOfTree = 0;
        }
        public KeyValuePair<double, T> PeekOfHeap()
        {
            if (arr.Count <1)
                throw new IndexOutOfRangeException();
            else
                return new KeyValuePair<double, T>(arr[1],arr2[1]);
        }
        public void Insert(double priority, T item)
        {
            arr.Add(priority);
            arr2.Add(item);
            sizeOfTree++;
            HeapifyBottomToTop(sizeOfTree);
        }
        private void HeapifyBottomToTop(int index)
        {
            int parent = index / 2;
            if (index <= 1)
            {
                return;
            }
            // If Current value is larger than its parent, then we need to swap  
            if (arr[index] > arr[parent])
            {
                swapElements(index, parent);
            }
            HeapifyBottomToTop(parent);
        }
        public KeyValuePair<double, T> extractHeadOfHeap()
        {
            if (sizeOfTree == 0)
            {
                throw new IndexOutOfRangeException();
            }
            else
            {
                swapElements(1, sizeOfTree);
                var extractedpair = new KeyValuePair<double, T>(arr.Last(), arr2.Last());
                arr.RemoveAt(sizeOfTree);
                arr2.RemoveAt(sizeOfTree);
                sizeOfTree--;
                HeapifyTopToBottom(1);
                return extractedpair;
            }
        }//end of method  
        private void HeapifyTopToBottom(int index)
        {
            int left = index * 2;
            int right = (index * 2) + 1;
            int largestChild;

            if (sizeOfTree < left)
            { //If there is no child of this node, then nothing to do. Just return.  
                return;
            }
            else if (sizeOfTree == left)
            { //If there is only left child of this node, then do a comparison and return.  
                if (arr[index] < arr[left])
                {
                    swapElements(index, left);
                }
                return;
            }
            else
            { //If both children are there  
               if (arr[left] > arr[right])
                { //Find out the smallest child  
                    largestChild = left;
                }
                else
                {
                    largestChild = right;
                }
                if (arr[index] > arr[largestChild])
                { //If Parent is greater than smallest child, then swap  
                    swapElements(index, largestChild);
                }
                HeapifyTopToBottom(largestChild);
            }
        }//end of method  
        void swapElements(int a, int b)
        {
            double tmp = arr[a];
            T tmp2 = arr2[a];
            arr[a] = arr[b];
            arr2[a] = arr2[b];
            arr[b] = tmp;
            arr2[b] = tmp2;
        }

    }
}
