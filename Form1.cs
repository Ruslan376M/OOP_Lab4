using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Лабораторная_работа__4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
			this.DoubleBuffered = true;
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
		}

        public class CCircle
        {
            public int x;
            public int y;
            public int radius;

            public CCircle()
            {
                x = 0;
                y = 0;
                radius = 0;
            }

            public CCircle(int x, int y, int radius)
            {
                this.x = x;
                this.y = y;
                this.radius = radius;
            }

            public CCircle(CCircle circle)
            {
                this.x = circle.x;
                this.y = circle.y;
                this.radius = circle.radius;
            }
        }

		public class Storage<T>
		{
			private class Node
			{
				public T obj;
				public Node previous;
				public Node next;
			}

			private int size;
			private Node first;
			private Node last;
			private Node current;
			public Storage()
			{
				size = 0;
			}

			public void add(T obj) // Добавляет объект в хранилище в конец списка
			{
				Node temp = new Node();
				temp.obj = obj;

				size++;

				if (first == null)
				{
					first = temp;
					last = temp;
					current = temp;
				}
				else
				{
					last.next = temp;
					temp.previous = last;
					last = temp;
				}
			}

			public void del() // Удаляет текущий элемент
			{
				if (current != null)
				{
					// Переназначение указателей соседних элементов 
					if (current.previous != null)
						current.previous.next = current.next;
					if (current.next != null)
						current.next.previous = current.previous;

					// Перевод current на следующий или предыдущий элемент
					Node oldCurrent = current;

					if (current.next != null)
						current = current.next;
					else if (current.previous != null)
						current = current.previous;
					else
						current = null;

					// Смена указателей first и last, если current был им равен
					if (oldCurrent == first)
						first = current;
					if (oldCurrent == last)
						last = current;

					// Удаление элемента из списка
					size--;
				}
			}

			public void previous() // Возвращает указатель current на предыдущий элемент в списке, если предыдущий элемент существует
			{
				if (current != null)
					if (current.previous != null)
						current = current.previous;
			}

			public void next() // Возвращает указатель current на следующий элемент в списке, если следующий элемент существует
			{
				if (current != null)
					if (current.next != null)
						current = current.next;
			}

			public bool check(T obj) // Проверяет наличие объекта с тем же указателем в хранилище
			{
				Node buffer = first;
				for (int i = 0; i < size; i++, buffer = buffer.next)
					if (buffer.obj.Equals(obj))
						return true;
				return false;
			}

			public int getSize()
			{
				return size;
			}

			public T getFirst() // Возвращает ссылку на первый объект в списке
			{
				return first.obj;
			}

			public T getLast() // Возвращает ссылку на последний объект в списке
			{
				return last.obj;
			}

			public T getCurrent() // Возвращает ссылку на текущий объект
			{
				return current.obj;
			}

			public void setFirst() // Устанавливает текущий указатель на начало списка
			{
				current = first;
			}

			public void setLast() // Устанавливает текущий указатель в конец списка
			{
				current = last;
			}

			public bool eol() // End Of List
			{
				if (current.next == null)
					return true;
				else
					return false;
			}

			~Storage()
			{
				current = first;
				for (int i = 0; i < size; i++)
				{
					Node buffer = current.next;
					current = buffer;
				}
			}

		};

		bool ctrlIsPressed;
		Storage<CCircle> storage;
		int radius = 15;

		Graphics g;
		Bitmap image;
		Pen circlePen = new Pen(Brushes.SteelBlue, 3);

		private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
			ctrlIsPressed = e.Control;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
			ctrlIsPressed = e.Control;
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
			if (storage == null)
			{
				storage = new Storage<CCircle>();
				image = new Bitmap(1920, 1080);
				g = Graphics.FromImage(image);
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			}

			if (e.Button == MouseButtons.Left && ctrlIsPressed)
            {

            }
			else if(e.Button == MouseButtons.Left)
            {
				storage.setFirst();
				for (int i = 0; i < storage.getSize(); i++, storage.next())
                {
					int x = storage.getCurrent().x;
					int y = storage.getCurrent().y;
					int radius = storage.getCurrent().radius;
					if ((x - e.X) * (x - e.X) + (y - e.Y) * (y - e.Y) <= 4 * radius * radius)
						return ;
                }
				storage.add(new CCircle(e.X, e.Y, radius));
				g.FillEllipse(Brushes.SteelBlue, e.X - radius, e.Y - radius, 2 * radius, 2 * radius);
				pictureBox.Image = image;
			}
        }
    }
}
