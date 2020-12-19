using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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

			public bool checkAndSetCurrent(T obj)
            {
				Node buffer = first;
				for (int i = 0; i < size; i++, buffer = buffer.next)
					if (buffer.obj.Equals(obj))
					{
						current = buffer;
						return true; 
					}
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
		};

		bool ctrlIsPressed;
		Storage<CCircle> storage;
		Storage<CCircle> selectedStorage;
		int radius = 15;
		bool inTheCircle;

		Graphics g;
		Bitmap image;
		Pen circlePen = new Pen(Brushes.Red, 5);

		private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
			ctrlIsPressed = e.Control;
			if (e.KeyCode == Keys.Delete)
				deleteSelected();
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
				selectedStorage = new Storage<CCircle>();
				image = new Bitmap(1920, 1080);
				g = Graphics.FromImage(image);
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			}

			if (e.Button == MouseButtons.Left && ctrlIsPressed)
            {
				CCircle circle = inTheAreaOfCircle(e.X, e.Y);
				if (circle != null)
				{
					if (inTheCircle)
					{
						if (selectedStorage.checkAndSetCurrent(circle) == true)
							deselectOne();
						else
						{
							selectedStorage.add(circle);
							printSelectedCircle(circle.x, circle.y, circle.radius);
						}
							
					}
				}
			}
			else if(e.Button == MouseButtons.Left)
            {
				deselectAll();
				CCircle circle = inTheAreaOfCircle(e.X, e.Y);
				if (circle == null)
                {
					circle = new CCircle(e.X, e.Y, radius);
					storage.add(circle);
					selectedStorage.add(circle);
					printSelectedCircle(circle.x, circle.y, circle.radius);
				}
                else 
				{
					if (inTheCircle)
					{
						selectedStorage.add(circle);
						printSelectedCircle(circle.x, circle.y, circle.radius);
					}
				}
			}
        }

		private void printCircle(int x, int y, int radius)
        {
			g.FillEllipse(Brushes.SteelBlue, x - radius, y - radius, 2 * radius, 2 * radius);
			//pictureBox.Image = image;
		}

		private void printSelectedCircle(int x, int y, int radius)
        {

			g.FillEllipse(Brushes.White, x - radius - 1, y - radius - 1, 2 * (radius + 1), 2 * (radius + 1));
			g.FillEllipse(Brushes.LightSkyBlue, x - radius - 1, y - radius - 1, 2 * (radius + 1), 2 * (radius + 1));
			//pictureBox.Image = image;
		}

		private void deselectPrintedCircle(int x, int y, int radius)
        {
			g.FillEllipse(Brushes.White, x - radius - 2, y - radius - 2, 2 * (radius + 2), 2 * (radius + 2));
			g.FillEllipse(Brushes.SteelBlue, x - radius, y - radius, 2 * radius, 2 * radius);
			//pictureBox.Image = image;
		}

		private CCircle inTheAreaOfCircle(int X, int Y)
        {
			storage.setFirst();
			for (int i = 0; i < storage.getSize(); i++, storage.next())
			{
				int x = storage.getCurrent().x;
				int y = storage.getCurrent().y;
				int radius = storage.getCurrent().radius;
				int temp = (x - X) * (x - X) + (y - Y) * (y - Y);
				if (temp <= 4 * radius * radius)
				{
					inTheCircle = (temp <= radius * radius);
					return storage.getCurrent(); 
				}
			}
			return null;
		}

		private void deselectOne()
        {
			CCircle circle = selectedStorage.getCurrent();
			deselectPrintedCircle(circle.x, circle.y, circle.radius);
			selectedStorage.del();
		}

		private void deselectAll()
		{
			selectedStorage.setFirst();
			for (int i = 0; i < selectedStorage.getSize(); i++, selectedStorage.next())
			{
				CCircle circle = selectedStorage.getCurrent();
				deselectPrintedCircle(circle.x, circle.y, circle.radius);
			}
			selectedStorage = new Storage<CCircle>();
		}

		private void deleteSelected()
        {
			selectedStorage.setFirst();
			for (int i = 0; i < selectedStorage.getSize(); i++, selectedStorage.next())
			{
				CCircle circle = selectedStorage.getCurrent();

				storage.setFirst();
				for (int j = 0; j < storage.getSize(); j++, storage.next())
					if (circle.Equals(storage.getCurrent()))
					{
						storage.del();
						break;
					}
				g.FillEllipse(Brushes.White, circle.x - radius - 2, circle.y - radius - 2, 2 * (circle.radius + 2), 2 * (circle.radius + 2));
			}
			selectedStorage = new Storage<CCircle>();
		}

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
			pictureBox.Image = image;
		}
    }
}
