using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadedBinarySearchTree
{

    class ThreadedBinarySearchTree
    {
        internal class Node
        {
            public int value;
            public Node left;
            public Node right;

            public Node(int value){
                this.value = value;
                this.left = null;
                this.right = null;
            }
        }

        private Node root;
        //private  Mutex bstMutex = new Mutex();

        private SemaphoreSlim rw_mutex = new SemaphoreSlim(1);
        private SemaphoreSlim mutex = new SemaphoreSlim(1);
        private int read_count = 0;

        public ThreadedBinarySearchTree(){
            root = null;
        }

        //writer
        public void add(int num) {
            rw_mutex.Wait();

            Node parent = null;
            Node child = root;
            while (child != null) { 
                parent = child;
                if (num < child.value)
                {
                    child = child.left;
                }
                else if (num>child.value)
                {
                    child = child.right;
                }
                else
                {
                    return;
                }
            }
            Node newNode = new Node(num);
            
            if (root == null)
            {
                root = newNode;
            }
            else
            {
                if (num < parent.value)
                {
                    parent.left = newNode;
                }
                else
                {
                    parent.right = newNode;
                }
            }

            rw_mutex.Release();

        }


        private Node remove(Node root, int value)
        {
            /* Base Case: If the tree is empty */
            if (root == null)
                return root;

            /* Otherwise, recur down the tree */
            if (value < root.value)
                root.left = remove(root.left, value);
            else if (value > root.value)
                root.right = remove(root.right, value);
            
            // if key is same as root's key, then This is the
            // node to be deleted
            else
            {
                // node with only one child or no child
                if (root.left == null)
                    return root.right;
                else if (root.right == null)
                    return root.left;

                // node with two children: Get the
                // inorder successor (smallest
                // in the right subtree)
                root.value = minValue(root.right);

                // Delete the inorder successor
                root.right = remove(root.right, root.value);
            }
            return root;
        }

        private int minValue(Node root)
        {
            int minv = root.value;
            while (root.left != null)
            {
                minv = root.left.value;
                root = root.left;
            }
            return minv;
        }

        //writer
        public void remove(int num) {
            rw_mutex.Wait();
            remove(root, num);
            rw_mutex.Release();
        } 


        //reader
        public bool search(int num) {
            mutex.Wait();
            read_count++;
            //first reader enterted 
            if (read_count == 1)
                rw_mutex.Wait();
            mutex.Release();

            bool result = false;
            Node parent = null;
            Node child = root;
            while (child != null)
            {
                parent = child;
                if (num < child.value)
                {
                    child = child.left;
                }
                else if (num > child.value)
                {
                    child = child.right;
                }
                else
                {
                    Console.WriteLine("Found: "+ num.ToString());
                    result = true;
                    break;
                }
            }
            
            mutex.Wait();
            read_count--;
            //last reader 
            if (read_count == 0)
                rw_mutex.Release();
            mutex.Release();

            return result;
        } 
        

        //writer
        public void clear() {
            rw_mutex.Wait();

            root = null;

            rw_mutex.Release();
        } 

        private void print(Node node)
        {
            if (node != null)
            {
                print(node.left);
                Console.Write(node.value + " ");
                print(node.right);
            }
        }

        //reader
        public void print() {
            mutex.Wait();
            read_count++;
            //first reader enterted 
            if (read_count == 1)
                rw_mutex.Wait();
            mutex.Release();


            print(root);
            Console.WriteLine();


            mutex.Wait();
            read_count--;
            //last reader 
            if (read_count == 0)
                rw_mutex.Release();
            mutex.Release();
        } 

    }



    internal class Program
    {
        static void Main(string[] args)
        {
            ThreadedBinarySearchTree bst = new ThreadedBinarySearchTree();
            Thread t1 = new Thread(() => bst.add(3));
            Thread t2 = new Thread(() => bst.add(5));
            Thread t3 = new Thread(() => bst.add(7));
            Thread t4 = new Thread(() => bst.add(9));
            Thread t5 = new Thread(() => bst.add(12));
            Thread t6 = new Thread(() => bst.add(15));
            Thread t7 = new Thread(() => bst.add(17));
            Thread t8 = new Thread(() => bst.add(21));
            Thread t9 = new Thread(() => bst.add(4));
            Thread t10 = new Thread(() => bst.add(1));
            Thread t11 = new Thread(() => bst.add(0));
            Thread t12 = new Thread(() => bst.add(8));
            Thread t13 = new Thread(() => bst.add(11));
            Thread t14 = new Thread(() => bst.remove(5));
            Thread t15 = new Thread(() => bst.remove(0));
            Thread t19 = new Thread(() => bst.search(0));
            Thread t20 = new Thread(() => bst.search(5));
            Thread t21 = new Thread(() => bst.search(15));
            Thread t22 = new Thread(() => bst.search(21));
            Thread t16 = new Thread(() => bst.remove(4));
            Thread t23 = new Thread(() => bst.search(4));
            Thread t17 = new Thread(() => bst.remove(21));
            Thread t24 = new Thread(() => bst.search(3));
            Thread t25 = new Thread(() => bst.search(9));
            Thread t26 = new Thread(() => bst.search(12));
            Thread t18 = new Thread(() => bst.clear());

            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
            t5.Start();
            t6.Start();
            t7.Start();
            t8.Start();
            t9.Start();
            t10.Start();
            t11.Start();
            t12.Start();
            t13.Start();
            t14.Start();
            t15.Start();
            t16.Start();
            t17.Start();
            t19.Start();
            t20.Start();
            t21.Start();
            t22.Start();
            t23.Start();
            t24.Start();
            t25.Start();
            t26.Start();
            t18.Start();

            t1.Join();
            t2.Join();
            t3.Join();
            t4.Join();
            t5.Join();
            t6.Join();
            t7.Join();
            t8.Join();
            t9.Join();
            t10.Join();
            t11.Join();
            t12.Join();
            t13.Join();
            t14.Join();
            t15.Join();
            t16.Join();
            t17.Join();
            t18.Join();
            t19.Join();
            t20.Join();
            t21.Join();
            t22.Join();
            t23.Join();
            t24.Join();
            t25.Join();
            t26.Join();

            //bst.print();
            

        }
    }
}
