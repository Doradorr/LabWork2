using System;
using System.Collections;
using System.Collections.Generic;

namespace VectorTreeExample
{
    // ---------------- КЛАС ВЕКТОРА ----------------
    public class Vector : IComparable<Vector>
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Vector(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        // Обчислення довжини вектора
        public double Length() => Math.Sqrt(X * X + Y * Y + Z * Z);

        // Порівняння за довжиною
        public int CompareTo(Vector other)
        {
            return this.Length().CompareTo(other.Length());
        }

        public override string ToString()
        {
            return $"Vector({X}, {Y}, {Z}) | Length = {Length():F2}";
        }
    }

    // ---------------- УЗАГАЛЬНЕНЕ БІНАРНЕ ДЕРЕВО ----------------
    public class Node<T>
    {
        public T Data;
        public Node<T> Left;
        public Node<T> Right;

        public Node(T data) => Data = data;
    }

    public class BinaryTree<T> : IEnumerable<T> where T : IComparable<T>
    {
        private Node<T> root;

        public void Insert(T data)
        {
            root = InsertRec(root, data);
        }

        private Node<T> InsertRec(Node<T> node, T data)
        {
            if (node == null) return new Node<T>(data);

            if (data.CompareTo(node.Data) < 0)
                node.Left = InsertRec(node.Left, data);
            else
                node.Right = InsertRec(node.Right, data);

            return node;
        }

        // Обхід дерева у прямому порядку (Preorder)
        public void Preorder(Node<T> node)
        {
            if (node == null) return;
            Console.WriteLine(node.Data);
            Preorder(node.Left);
            Preorder(node.Right);
        }

        public void DisplayPreorder()
        {
            Console.WriteLine("\nPreorder traversal:");
            Preorder(root);
        }

        // Реалізація ітератора
        public IEnumerator<T> GetEnumerator()
        {
            return PreorderEnumerator(root).GetEnumerator();
        }

        private IEnumerable<T> PreorderEnumerator(Node<T> node)
        {
            if (node == null) yield break;
            yield return node.Data;
            foreach (var left in PreorderEnumerator(node.Left)) yield return left;
            foreach (var right in PreorderEnumerator(node.Right)) yield return right;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    // ---------------- ОСНОВНА ПРОГРАМА ----------------
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // 1️⃣ Масив векторів
            Vector[] array = {
                new Vector(1, 2, 3),
                new Vector(3, 4, 5),
                new Vector(0, 1, 1)
            };

            Console.WriteLine("Масив векторів:");
            foreach (var v in array)
                Console.WriteLine(v);

            // 2️⃣ Generic List
            List<Vector> list = new List<Vector>(array);
            list.Add(new Vector(2, 2, 2));
            list.RemoveAt(0);
            list[0] = new Vector(5, 5, 5);

            Console.WriteLine("\nList (узагальнена колекція):");
            foreach (var v in list)
                Console.WriteLine(v);

            // 3️⃣ Non-generic ArrayList
            ArrayList arrayList = new ArrayList(list);
            arrayList.Add(new Vector(7, 7, 7));
            arrayList.RemoveAt(1);

            Console.WriteLine("\nArrayList (неузагальнена колекція):");
            foreach (var v in arrayList)
                Console.WriteLine(v);

            // 4️⃣ Бінарне дерево векторів
            BinaryTree<Vector> tree = new BinaryTree<Vector>();
            foreach (var v in list)
                tree.Insert(v);

            tree.DisplayPreorder();

            // 5️⃣ Демонстрація ітератора
            Console.WriteLine("\nПеребір дерева через foreach:");
            foreach (var v in tree)
                Console.WriteLine(v);
        }
    }
}
