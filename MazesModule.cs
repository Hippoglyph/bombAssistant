using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombAssistant
{
    class MazesModule
    {
        Speaker talk;
        Listener rec;
        String exit;
        int[] circle;
        int[] player;
        int[] goal;
        bool running = true;

        struct node
        {
            public int id;
            public int x;
            public int y;
            public node[] neighbors;
            public bool circle;
        }

        public MazesModule(Speaker talk, Listener rec)
        {
            this.talk = talk;
            this.rec = rec;
            exit = "Coordinates contained 0. Ready for new module";
            circle = new int[2] { -1, -1 };
            player = new int[2] { -1, -1 };
            goal = new int[2] { -1, -1 };
            solve();
        }

        private void solve()
        {
            if (!init())
                return;
            node[,] maze = getMaze();
            if (!running)
                return;
            Dictionary<int, node> path = pathFind(maze);
            StringBuilder sb = new StringBuilder();
            sb.Append("Follow sequence: ");
            sb.Append(getStringPath(path,maze[player[0],player[1]], maze[goal[0],goal[1]]));
            sb.Remove(sb.Length - 2, 2);
            talk.speakAsync(sb.ToString());
        }

        private bool init()
        {
            while (circle[0] == -1)
            {
                talk.speakAsync("What is the coordinates of one circle?");
                circle = rec.getCoords();
                if (hasZero(circle))
                {
                    talk.speakAsync(exit);
                    return false;
                }
            }
            circle[0]--;
            circle[1]--;
            while (player[0] == -1)
            {
                talk.speakAsync("What is your position?");
                player = rec.getCoords();
                if (hasZero(player))
                {
                    talk.speakAsync(exit);
                    return false;
                }
            }
            player[0]--;
            player[1]--;
            while (goal[0] == -1)
            {
                talk.speakAsync("What is the position of the goal?");
                goal = rec.getCoords();
                if (hasZero(goal))
                {
                    talk.speakAsync(exit);
                    return false;
                }
            }
            goal[0]--;
            goal[1]--;
            return true;
        }

        private String getStringPath(Dictionary<int, node> path, node current, node goal)
        {
            String direction = "";
            if (current.id == goal.id)
                return direction;
            node next = path[current.id];
            if (next.x < current.x)
                direction = "left, ";
            else if (next.x > current.x)
                direction = "right, ";
            else if (next.y < current.y)
                direction = "up, ";
            else if (next.y > current.y)
                direction = "down, ";
            return direction + getStringPath(path, next, goal);
        }

        private Dictionary<int, node> pathFind(node[,] maze)
        {
            int[,] dist = new int[maze.GetLength(0),maze.GetLength(1)];
            Dictionary<int, node> prev = new Dictionary<int, node>(maze.Length);
            List<node> queue = new List<node>();

            foreach(node node_ in maze)
            {
                dist[node_.x, node_.y] = -1;
                queue.Add(node_);
            }
            dist[goal[0], goal[1]] = 0;

            while (queue.Count != 0)
            {
                node u = getMinDist(queue, dist);
                queue.Remove(u);

                foreach(node v in u.neighbors)
                {
                    int alt = dist[u.x, u.y] + 1;
                    if (alt < dist[v.x, v.y] || dist[v.x, v.y] == -1)
                    {
                        dist[v.x, v.y] = alt;
                        if(prev.ContainsKey(v.id))
                            prev.Remove(v.id);
                        prev.Add(v.id, u);
                    }
                }
            }
            return prev;
        }

        private node getMinDist(List<node> queue, int[,] dist)
        {
            node minNode = new node();
            foreach(node node_ in queue)
            {
                if (dist[node_.x, node_.y] != -1)
                {
                    minNode = node_;
                    break;
                }
            }

            foreach(node node_ in queue)
            {
                if (dist[node_.x, node_.y] == -1)
                    continue;
                if (dist[node_.x, node_.y] < dist[minNode.x, minNode.y])
                    minNode = node_;
            }
            return minNode;
        }

        private bool hasZero(int[] coord)
        {
            if (coord[0] == 0 || coord[1] == 0)
                return true;
            return false;
        }

        private node[,] getMaze()
        {
            node[,] maze;

            /*
            maze = getTestMaze();
            if (validate(maze))
                return maze;
            */
            maze = getMaze0();
            if (validate(maze))
                return maze;
            maze = getMaze1();
            if (validate(maze))
                return maze;
            maze = getMaze2();
            if (validate(maze))
                return maze;
            maze = getMaze3();
            if (validate(maze))
                return maze;
            maze = getMaze4();
            if (validate(maze))
                return maze;
            maze = getMaze5();
            if (validate(maze))
                return maze;
            maze = getMaze6();
            if (validate(maze))
                return maze;
            maze = getMaze7();
            if (validate(maze))
                return maze;
            maze = getMaze8();
            if (validate(maze))
                return maze;

            talk.speakAsync("Maze does not exits. Please repeat module!");
            running = false;
            return maze;
        }

        private bool validate(node[,] maze)
        {
            if (maze[circle[0], circle[1]].circle)
                return true;
            return false;
        }

        private node[,] getTestMaze()
        {
            node[,] nodes = new node[2, 2];
            nodes[0, 0].x = 0;
            nodes[0, 0].y = 0;
            nodes[0, 0].id = 0;
            nodes[1, 0].x = 1;
            nodes[1, 0].y = 0;
            nodes[1, 0].id = 1;
            nodes[0, 1].x = 0;
            nodes[0, 1].y = 1;
            nodes[0, 1].id = 2;
            nodes[1, 1].x = 1;
            nodes[1, 1].y = 1;
            nodes[1, 1].id = 3;

            nodes[0, 0].neighbors = new node[] { nodes[1, 0], nodes[0, 1] };
            nodes[0, 1].neighbors = new node[] { nodes[0, 0] };
            nodes[1, 0].neighbors = new node[] { nodes[0, 0], nodes[1, 1] };
            nodes[1, 1].neighbors = new node[] { nodes[1, 0] };

            nodes[0, 0].circle = true;
            return nodes;
        }

        private node[,] getMaze0()
        {
            node[,] nodes = createInitNodes();

            nodes[0, 0].neighbors = new node[] { nodes[1,0], nodes[0,1] };
            nodes[0, 1].neighbors = new node[] { nodes[0,0], nodes[0,2] };
            nodes[0, 2].neighbors = new node[] { nodes[0,1], nodes[0,3] };
            nodes[0, 3].neighbors = new node[] { nodes[0,2], nodes[0,4] };
            nodes[0, 4].neighbors = new node[] { nodes[0,3], nodes[0,5], nodes[1,4] };
            nodes[0, 5].neighbors = new node[] { nodes[0,4], nodes[1,5] };

            nodes[1, 0].neighbors = new node[] { nodes[0,0], nodes[2,0] };
            nodes[1, 1].neighbors = new node[] { nodes[2,1], nodes[1,2] };
            nodes[1, 2].neighbors = new node[] { nodes[1,1], nodes[2,2] };
            nodes[1, 3].neighbors = new node[] { nodes[2,3] };
            nodes[1, 4].neighbors = new node[] { nodes[0,4], nodes[2,4] };
            nodes[1, 5].neighbors = new node[] { nodes[0,5] };

            nodes[2, 0].neighbors = new node[] { nodes[1,0], nodes[2,1] };
            nodes[2, 1].neighbors = new node[] { nodes[2,0], nodes[1,1] };
            nodes[2, 2].neighbors = new node[] { nodes[1,2], nodes[2,3] };
            nodes[2, 3].neighbors = new node[] { nodes[1,3], nodes[2,2], nodes[3,3] };
            nodes[2, 4].neighbors = new node[] { nodes[1,4], nodes[2,5] };
            nodes[2, 5].neighbors = new node[] { nodes[2,4], nodes[3,5] };

            nodes[3, 0].neighbors = new node[] { nodes[4,0], nodes[3,1] };
            nodes[3, 1].neighbors = new node[] { nodes[3,0], nodes[4,1] };
            nodes[3, 2].neighbors = new node[] { nodes[3,3], nodes[4,2] };
            nodes[3, 3].neighbors = new node[] { nodes[2,3], nodes[3,2] };
            nodes[3, 4].neighbors = new node[] { nodes[4,4], nodes[3,5] };
            nodes[3, 5].neighbors = new node[] { nodes[3,4], nodes[2,5] };

            nodes[4, 0].neighbors = new node[] { nodes[3,0], nodes[5,0] };
            nodes[4, 1].neighbors = new node[] { nodes[3,1], nodes[5,1] };
            nodes[4, 2].neighbors = new node[] { nodes[3,2], nodes[5,2] };
            nodes[4, 3].neighbors = new node[] { nodes[5,3] };
            nodes[4, 4].neighbors = new node[] { nodes[3,4] };
            nodes[4, 5].neighbors = new node[] { nodes[5,5] };

            nodes[5, 0].neighbors = new node[] { nodes[4,0] };
            nodes[5, 1].neighbors = new node[] { nodes[4,1], nodes[5,2] };
            nodes[5, 2].neighbors = new node[] { nodes[5,1], nodes[5,3], nodes[4,2] };
            nodes[5, 3].neighbors = new node[] { nodes[5,2], nodes[5,4], nodes[4,3] };
            nodes[5, 4].neighbors = new node[] { nodes[5,3], nodes[5,5] };
            nodes[5, 5].neighbors = new node[] { nodes[4,5], nodes[5,4] };

            nodes[0, 1].circle = true;
            nodes[5, 2].circle = true;

            return nodes;
        }

        private node[,] getMaze1()
        {
            node[,] nodes = createInitNodes();

            nodes[0, 0].neighbors = new node[] { nodes[1, 0] };
            nodes[0, 1].neighbors = new node[] { nodes[0, 2], nodes[1, 1] };
            nodes[0, 2].neighbors = new node[] { nodes[0, 1], nodes[0, 3] };
            nodes[0, 3].neighbors = new node[] { nodes[0, 2], nodes[0, 4], nodes[1, 3] };
            nodes[0, 4].neighbors = new node[] { nodes[0, 3], nodes[0, 5] };
            nodes[0, 5].neighbors = new node[] { nodes[0, 4] };

            nodes[1, 0].neighbors = new node[] { nodes[0, 0], nodes[2, 0], nodes[1,1] };
            nodes[1, 1].neighbors = new node[] { nodes[1, 0], nodes[0, 1] };
            nodes[1, 2].neighbors = new node[] { nodes[1, 3], nodes[2, 2] };
            nodes[1, 3].neighbors = new node[] { nodes[1, 2], nodes[0, 3] };
            nodes[1, 4].neighbors = new node[] { nodes[1, 5] };
            nodes[1, 5].neighbors = new node[] { nodes[1, 4], nodes[2, 5] };

            nodes[2, 0].neighbors = new node[] { nodes[1, 0] };
            nodes[2, 1].neighbors = new node[] { nodes[2, 2], nodes[3, 1] };
            nodes[2, 2].neighbors = new node[] { nodes[1, 2], nodes[2, 1] };
            nodes[2, 3].neighbors = new node[] { nodes[2, 4], nodes[3, 3] };
            nodes[2, 4].neighbors = new node[] { nodes[2, 3], nodes[2, 5] };
            nodes[2, 5].neighbors = new node[] { nodes[1, 5], nodes[2, 4] };

            nodes[3, 0].neighbors = new node[] { nodes[4, 0], nodes[3, 1] };
            nodes[3, 1].neighbors = new node[] { nodes[3, 0], nodes[2, 1] };
            nodes[3, 2].neighbors = new node[] { nodes[3, 3], nodes[4, 2] };
            nodes[3, 3].neighbors = new node[] { nodes[2, 3], nodes[3, 2] };
            nodes[3, 4].neighbors = new node[] { nodes[4, 4], nodes[3, 5] };
            nodes[3, 5].neighbors = new node[] { nodes[3, 4], nodes[4, 5] };

            nodes[4, 0].neighbors = new node[] { nodes[3, 0], nodes[5, 0], nodes[4, 1] };
            nodes[4, 1].neighbors = new node[] { nodes[4, 0], nodes[5, 1] };
            nodes[4, 2].neighbors = new node[] { nodes[3, 2], nodes[5, 2] };
            nodes[4, 3].neighbors = new node[] { nodes[4, 4] };
            nodes[4, 4].neighbors = new node[] { nodes[4, 3], nodes[3, 4] };
            nodes[4, 5].neighbors = new node[] { nodes[5, 5], nodes[3, 5] };

            nodes[5, 0].neighbors = new node[] { nodes[4, 0] };
            nodes[5, 1].neighbors = new node[] { nodes[4, 1], nodes[5, 2] };
            nodes[5, 2].neighbors = new node[] { nodes[5, 1], nodes[5, 3], nodes[4, 2] };
            nodes[5, 3].neighbors = new node[] { nodes[5, 2], nodes[5, 4] };
            nodes[5, 4].neighbors = new node[] { nodes[5, 3], nodes[5, 5] };
            nodes[5, 5].neighbors = new node[] { nodes[4, 5], nodes[5, 4] };

            nodes[4, 1].circle = true;
            nodes[1, 3].circle = true;

            return nodes;
        }

        private node[,] getMaze2()
        {
            node[,] nodes = createInitNodes();

            nodes[0, 0].neighbors = new node[] { nodes[1, 0], nodes[0, 1] };
            nodes[0, 1].neighbors = new node[] { nodes[0, 0] };
            nodes[0, 2].neighbors = new node[] { nodes[0, 3], nodes[1, 2] };
            nodes[0, 3].neighbors = new node[] { nodes[0, 2], nodes[0, 4] };
            nodes[0, 4].neighbors = new node[] { nodes[0, 3], nodes[0, 5] };
            nodes[0, 5].neighbors = new node[] { nodes[0, 4], nodes[1, 5] };

            nodes[1, 0].neighbors = new node[] { nodes[0, 0], nodes[2, 0] };
            nodes[1, 1].neighbors = new node[] { nodes[1, 2] };
            nodes[1, 2].neighbors = new node[] { nodes[1, 3], nodes[0, 2], nodes[1, 1] };
            nodes[1, 3].neighbors = new node[] { nodes[1, 2], nodes[1, 4] };
            nodes[1, 4].neighbors = new node[] { nodes[1, 3], nodes[2, 4] };
            nodes[1, 5].neighbors = new node[] { nodes[0, 5], nodes[2, 5] };

            nodes[2, 0].neighbors = new node[] { nodes[1, 0], nodes[2, 1] };
            nodes[2, 1].neighbors = new node[] { nodes[2, 2], nodes[2, 0] };
            nodes[2, 2].neighbors = new node[] { nodes[2, 1], nodes[2, 3] };
            nodes[2, 3].neighbors = new node[] { nodes[2, 4], nodes[2, 2] };
            nodes[2, 4].neighbors = new node[] { nodes[2, 3], nodes[1, 4] };
            nodes[2, 5].neighbors = new node[] { nodes[1, 5], nodes[3, 5] };

            nodes[3, 0].neighbors = new node[] { nodes[3, 1] };
            nodes[3, 1].neighbors = new node[] { nodes[3, 0], nodes[4, 1] };
            nodes[3, 2].neighbors = new node[] { nodes[3, 3], nodes[4, 2] };
            nodes[3, 3].neighbors = new node[] { nodes[3, 2], nodes[3, 4] };
            nodes[3, 4].neighbors = new node[] { nodes[3, 3], nodes[3, 5] };
            nodes[3, 5].neighbors = new node[] { nodes[3, 4], nodes[2, 5] };

            nodes[4, 0].neighbors = new node[] { nodes[5, 0], nodes[4, 1] };
            nodes[4, 1].neighbors = new node[] { nodes[4, 0], nodes[3, 1] };
            nodes[4, 2].neighbors = new node[] { nodes[3, 2], nodes[4, 3] };
            nodes[4, 3].neighbors = new node[] { nodes[4, 2], nodes[4, 4] };
            nodes[4, 4].neighbors = new node[] { nodes[4, 3], nodes[4, 5] };
            nodes[4, 5].neighbors = new node[] { nodes[5, 5], nodes[4, 4] };

            nodes[5, 0].neighbors = new node[] { nodes[4, 0], nodes[5, 1] };
            nodes[5, 1].neighbors = new node[] { nodes[5, 0], nodes[5, 2] };
            nodes[5, 2].neighbors = new node[] { nodes[5, 1], nodes[5, 3] };
            nodes[5, 3].neighbors = new node[] { nodes[5, 2], nodes[5, 4] };
            nodes[5, 4].neighbors = new node[] { nodes[5, 3], nodes[5, 5] };
            nodes[5, 5].neighbors = new node[] { nodes[4, 5], nodes[5, 4] };

            nodes[5, 3].circle = true;
            nodes[3, 3].circle = true;

            return nodes;
        }

        private node[,] getMaze3()
        {
            node[,] nodes = createInitNodes();

            nodes[0, 0].neighbors = new node[] { nodes[1, 0], nodes[0, 1] };
            nodes[0, 1].neighbors = new node[] { nodes[0, 0], nodes[0, 2] };
            nodes[0, 2].neighbors = new node[] { nodes[0, 3], nodes[0, 1] };
            nodes[0, 3].neighbors = new node[] { nodes[0, 2], nodes[0, 4] };
            nodes[0, 4].neighbors = new node[] { nodes[0, 3], nodes[0, 5], nodes[1, 4] };
            nodes[0, 5].neighbors = new node[] { nodes[0, 4], nodes[1, 5] };

            nodes[1, 0].neighbors = new node[] { nodes[0, 0], nodes[1, 1] };
            nodes[1, 1].neighbors = new node[] { nodes[1, 0], nodes[1, 2] };
            nodes[1, 2].neighbors = new node[] { nodes[1, 1], nodes[2, 2] };
            nodes[1, 3].neighbors = new node[] { nodes[2, 3] };
            nodes[1, 4].neighbors = new node[] { nodes[0, 4], nodes[2, 4] };
            nodes[1, 5].neighbors = new node[] { nodes[0, 5], nodes[2, 5] };

            nodes[2, 0].neighbors = new node[] { nodes[3, 0] };
            nodes[2, 1].neighbors = new node[] { nodes[2, 2], nodes[3, 1] };
            nodes[2, 2].neighbors = new node[] { nodes[2, 1], nodes[1, 2] };
            nodes[2, 3].neighbors = new node[] { nodes[1, 3], nodes[3, 3] };
            nodes[2, 4].neighbors = new node[] { nodes[1, 4], nodes[3, 4] };
            nodes[2, 5].neighbors = new node[] { nodes[1, 5] };

            nodes[3, 0].neighbors = new node[] { nodes[2, 0], nodes[4, 0] };
            nodes[3, 1].neighbors = new node[] { nodes[2, 1], nodes[4, 1] };
            nodes[3, 2].neighbors = new node[] { nodes[4, 2], nodes[3, 3] };
            nodes[3, 3].neighbors = new node[] { nodes[2, 3], nodes[4, 3], nodes[3, 2] };
            nodes[3, 4].neighbors = new node[] { nodes[2, 4], nodes[4, 4] };
            nodes[3, 5].neighbors = new node[] { nodes[4, 5] };

            nodes[4, 0].neighbors = new node[] { nodes[5, 0], nodes[3, 0] };
            nodes[4, 1].neighbors = new node[] { nodes[3, 1], nodes[5, 1] };
            nodes[4, 2].neighbors = new node[] { nodes[3, 2] };
            nodes[4, 3].neighbors = new node[] { nodes[3, 3], nodes[5, 3] };
            nodes[4, 4].neighbors = new node[] { nodes[3, 4], nodes[4, 5] };
            nodes[4, 5].neighbors = new node[] { nodes[3, 5], nodes[4, 4] };

            nodes[5, 0].neighbors = new node[] { nodes[4, 0], nodes[5, 1] };
            nodes[5, 1].neighbors = new node[] { nodes[5, 0], nodes[5, 2], nodes[4, 1] };
            nodes[5, 2].neighbors = new node[] { nodes[5, 1], nodes[5, 3] };
            nodes[5, 3].neighbors = new node[] { nodes[5, 2], nodes[5, 4], nodes[4, 3] };
            nodes[5, 4].neighbors = new node[] { nodes[5, 3], nodes[5, 5] };
            nodes[5, 5].neighbors = new node[] { nodes[5, 4] };

            nodes[0, 0].circle = true;
            nodes[0, 3].circle = true;

            return nodes;
        }

        private node[,] getMaze4()
        {
            node[,] nodes = createInitNodes();

            nodes[0, 0].neighbors = new node[] { nodes[1, 0] };
            nodes[0, 1].neighbors = new node[] { nodes[1, 0], nodes[0, 2] };
            nodes[0, 2].neighbors = new node[] { nodes[0, 3], nodes[0, 1], nodes[1, 2] };
            nodes[0, 3].neighbors = new node[] { nodes[0, 2], nodes[0, 4] };
            nodes[0, 4].neighbors = new node[] { nodes[0, 3], nodes[0, 5] };
            nodes[0, 5].neighbors = new node[] { nodes[0, 4] };

            nodes[1, 0].neighbors = new node[] { nodes[0, 0], nodes[2, 0] };
            nodes[1, 1].neighbors = new node[] { nodes[0, 1], nodes[2, 1] };
            nodes[1, 2].neighbors = new node[] { nodes[0, 2], nodes[1, 3] };
            nodes[1, 3].neighbors = new node[] { nodes[1, 2], nodes[2, 3] };
            nodes[1, 4].neighbors = new node[] { nodes[2, 4], nodes[1, 5] };
            nodes[1, 5].neighbors = new node[] { nodes[1, 4], nodes[2, 5] };
            
            nodes[2, 0].neighbors = new node[] { nodes[3, 0], nodes[1, 0] };
            nodes[2, 1].neighbors = new node[] { nodes[1, 1], nodes[3, 1] };
            nodes[2, 2].neighbors = new node[] { nodes[3, 1] };
            nodes[2, 3].neighbors = new node[] { nodes[1, 3], nodes[3, 3] };
            nodes[2, 4].neighbors = new node[] { nodes[1, 4], nodes[3, 4] };
            nodes[2, 5].neighbors = new node[] { nodes[1, 5], nodes[3, 5] };

            nodes[3, 0].neighbors = new node[] { nodes[2, 0], nodes[4, 0] };
            nodes[3, 1].neighbors = new node[] { nodes[2, 1], nodes[4, 1], nodes[3, 2] };
            nodes[3, 2].neighbors = new node[] { nodes[2, 2], nodes[3, 1] };
            nodes[3, 3].neighbors = new node[] { nodes[2, 3], nodes[3, 4] };
            nodes[3, 4].neighbors = new node[] { nodes[2, 4], nodes[3, 3], nodes[4, 4] };
            nodes[3, 5].neighbors = new node[] { nodes[4, 5], nodes[2, 5] };

            nodes[4, 0].neighbors = new node[] { nodes[5, 0], nodes[3, 0], nodes[4, 1] };
            nodes[4, 1].neighbors = new node[] { nodes[3, 1], nodes[4, 0] };
            nodes[4, 2].neighbors = new node[] { nodes[4, 3], nodes[5, 2] };
            nodes[4, 3].neighbors = new node[] { nodes[4, 2] };
            nodes[4, 4].neighbors = new node[] { nodes[3, 4] };
            nodes[4, 5].neighbors = new node[] { nodes[3, 5], nodes[5, 5] };

            nodes[5, 0].neighbors = new node[] { nodes[4, 0], nodes[5, 1] };
            nodes[5, 1].neighbors = new node[] { nodes[5, 0] };
            nodes[5, 2].neighbors = new node[] { nodes[4, 2], nodes[5, 3] };
            nodes[5, 3].neighbors = new node[] { nodes[5, 2], nodes[5, 4] };
            nodes[5, 4].neighbors = new node[] { nodes[5, 3], nodes[5, 5] };
            nodes[5, 5].neighbors = new node[] { nodes[5, 4], nodes[4, 5] };

            nodes[4, 2].circle = true;
            nodes[3, 5].circle = true;

            return nodes;
        }

        private node[,] getMaze5()
        {
            node[,] nodes = createInitNodes();

            nodes[0, 0].neighbors = new node[] { nodes[0, 1] };
            nodes[0, 1].neighbors = new node[] { nodes[0, 0], nodes[0, 2] };
            nodes[0, 2].neighbors = new node[] { nodes[0, 3], nodes[0, 1], nodes[1, 2] };
            nodes[0, 3].neighbors = new node[] { nodes[0, 2], nodes[1, 3] };
            nodes[0, 4].neighbors = new node[] { nodes[1, 4], nodes[0, 5] };
            nodes[0, 5].neighbors = new node[] { nodes[0, 4], nodes[1, 5] };

            nodes[1, 0].neighbors = new node[] { nodes[1, 1], nodes[2, 0] };
            nodes[1, 1].neighbors = new node[] { nodes[1, 0], nodes[1, 2] };
            nodes[1, 2].neighbors = new node[] { nodes[0, 2], nodes[1, 1] };
            nodes[1, 3].neighbors = new node[] { nodes[0, 3], nodes[1, 4] };
            nodes[1, 4].neighbors = new node[] { nodes[1, 3], nodes[0, 4] };
            nodes[1, 5].neighbors = new node[] { nodes[0, 5], nodes[2, 5] };

            nodes[2, 0].neighbors = new node[] { nodes[2, 1], nodes[1, 0] };
            nodes[2, 1].neighbors = new node[] { nodes[2, 0], nodes[2, 2] };
            nodes[2, 2].neighbors = new node[] { nodes[2, 1] };
            nodes[2, 3].neighbors = new node[] { nodes[3, 3], nodes[2, 4] };
            nodes[2, 4].neighbors = new node[] { nodes[2, 3] };
            nodes[2, 5].neighbors = new node[] { nodes[1, 5], nodes[3, 5] };

            nodes[3, 0].neighbors = new node[] { nodes[4, 0] };
            nodes[3, 1].neighbors = new node[] { nodes[4, 1], nodes[3, 2] };
            nodes[3, 2].neighbors = new node[] { nodes[3, 3], nodes[3, 1] };
            nodes[3, 3].neighbors = new node[] { nodes[2, 3], nodes[3, 4], nodes[3, 2] };
            nodes[3, 4].neighbors = new node[] { nodes[3, 5], nodes[3, 3] };
            nodes[3, 5].neighbors = new node[] { nodes[3, 4], nodes[2, 5] };

            nodes[4, 0].neighbors = new node[] { nodes[5, 0], nodes[3, 0], nodes[4, 1] };
            nodes[4, 1].neighbors = new node[] { nodes[3, 1], nodes[4, 0] };
            nodes[4, 2].neighbors = new node[] { nodes[4, 3], nodes[5, 2] };
            nodes[4, 3].neighbors = new node[] { nodes[4, 2], nodes[4, 4] };
            nodes[4, 4].neighbors = new node[] { nodes[5, 4], nodes[4, 3] };
            nodes[4, 5].neighbors = new node[] { nodes[5, 5] };

            nodes[5, 0].neighbors = new node[] { nodes[4, 0], nodes[5, 1] };
            nodes[5, 1].neighbors = new node[] { nodes[5, 0], nodes[5, 2] };
            nodes[5, 2].neighbors = new node[] { nodes[5, 1], nodes[4, 2] };
            nodes[5, 3].neighbors = new node[] { nodes[5, 4] };
            nodes[5, 4].neighbors = new node[] { nodes[5, 3], nodes[5, 5], nodes[4, 4] };
            nodes[5, 5].neighbors = new node[] { nodes[5, 4], nodes[4, 5] };

            nodes[4, 0].circle = true;
            nodes[2, 4].circle = true;

            return nodes;
        }

        private node[,] getMaze6()
        {
            node[,] nodes = createInitNodes();

            nodes[0, 0].neighbors = new node[] { nodes[0, 1], nodes[1, 0] };
            nodes[0, 1].neighbors = new node[] { nodes[0, 0], nodes[0, 2] };
            nodes[0, 2].neighbors = new node[] { nodes[0, 1], nodes[1, 2] };
            nodes[0, 3].neighbors = new node[] { nodes[0, 4], nodes[1, 3] };
            nodes[0, 4].neighbors = new node[] { nodes[0, 3], nodes[0, 5] };
            nodes[0, 5].neighbors = new node[] { nodes[0, 4], nodes[1, 5] };

            nodes[1, 0].neighbors = new node[] { nodes[0, 0], nodes[2, 0] };
            nodes[1, 1].neighbors = new node[] { nodes[1, 2], nodes[2, 1] };
            nodes[1, 2].neighbors = new node[] { nodes[0, 2], nodes[1, 1] };
            nodes[1, 3].neighbors = new node[] { nodes[0, 3], nodes[1, 4] };
            nodes[1, 4].neighbors = new node[] { nodes[1, 3] };
            nodes[1, 5].neighbors = new node[] { nodes[0, 5], nodes[2, 5] };
            
            nodes[2, 0].neighbors = new node[] { nodes[3, 0], nodes[1, 0] };
            nodes[2, 1].neighbors = new node[] { nodes[1, 1] };
            nodes[2, 2].neighbors = new node[] { nodes[2, 3], nodes[3, 2] };
            nodes[2, 3].neighbors = new node[] { nodes[3, 3], nodes[2, 4], nodes[2, 2] };
            nodes[2, 4].neighbors = new node[] { nodes[2, 3], nodes[3, 4] };
            nodes[2, 5].neighbors = new node[] { nodes[1, 5], nodes[3, 5] };

            nodes[3, 0].neighbors = new node[] { nodes[2, 0], nodes[3, 1] };
            nodes[3, 1].neighbors = new node[] { nodes[4, 1], nodes[3, 0] };
            nodes[3, 2].neighbors = new node[] { nodes[2, 2] };
            nodes[3, 3].neighbors = new node[] { nodes[2, 3], nodes[4, 3] };
            nodes[3, 4].neighbors = new node[] { nodes[2, 4], nodes[4, 4] };
            nodes[3, 5].neighbors = new node[] { nodes[2, 5], nodes[4, 5] };

            nodes[4, 0].neighbors = new node[] { nodes[5, 0], nodes[4, 1] };
            nodes[4, 1].neighbors = new node[] { nodes[3, 1], nodes[4, 0] };
            nodes[4, 2].neighbors = new node[] { nodes[4, 3], nodes[5, 2] };
            nodes[4, 3].neighbors = new node[] { nodes[4, 2], nodes[3, 3] };
            nodes[4, 4].neighbors = new node[] { nodes[3, 4], nodes[4, 5] };
            nodes[4, 5].neighbors = new node[] { nodes[5, 5], nodes[3, 5], nodes[4, 4] };

            nodes[5, 0].neighbors = new node[] { nodes[4, 0], nodes[5, 1] };
            nodes[5, 1].neighbors = new node[] { nodes[5, 0], nodes[5, 2] };
            nodes[5, 2].neighbors = new node[] { nodes[5, 1], nodes[4, 2] };
            nodes[5, 3].neighbors = new node[] { nodes[5, 4] };
            nodes[5, 4].neighbors = new node[] { nodes[5, 3], nodes[5, 5] };
            nodes[5, 5].neighbors = new node[] { nodes[5, 4], nodes[4, 5] };

            nodes[1, 0].circle = true;
            nodes[1, 5].circle = true;

            return nodes;
        }

        private node[,] getMaze7()
        {
            node[,] nodes = createInitNodes();

            nodes[0, 0].neighbors = new node[] { nodes[0, 1] };
            nodes[0, 1].neighbors = new node[] { nodes[0, 0], nodes[0, 2], nodes[1, 1] };
            nodes[0, 2].neighbors = new node[] { nodes[0, 1], nodes[0, 3] };
            nodes[0, 3].neighbors = new node[] { nodes[0, 4], nodes[0, 2] };
            nodes[0, 4].neighbors = new node[] { nodes[0, 3], nodes[0, 5] };
            nodes[0, 5].neighbors = new node[] { nodes[0, 4], nodes[1, 5] };

            nodes[1, 0].neighbors = new node[] { nodes[1, 1], nodes[2, 0] };
            nodes[1, 1].neighbors = new node[] { nodes[0, 1], nodes[2, 1], nodes[1, 0] };
            nodes[1, 2].neighbors = new node[] { nodes[2, 2], nodes[1, 3] };
            nodes[1, 3].neighbors = new node[] { nodes[1, 2], nodes[2, 3] };
            nodes[1, 4].neighbors = new node[] { nodes[1, 5] };
            nodes[1, 5].neighbors = new node[] { nodes[0, 5], nodes[2, 5], nodes[1, 4] };

            nodes[2, 0].neighbors = new node[] { nodes[3, 0], nodes[1, 0] };
            nodes[2, 1].neighbors = new node[] { nodes[1, 1] };
            nodes[2, 2].neighbors = new node[] { nodes[3, 2], nodes[1, 2] };
            nodes[2, 3].neighbors = new node[] { nodes[1, 3], nodes[2, 4] };
            nodes[2, 4].neighbors = new node[] { nodes[2, 3], nodes[3, 4] };
            nodes[2, 5].neighbors = new node[] { nodes[1, 5], nodes[3, 5] };

            nodes[3, 0].neighbors = new node[] { nodes[2, 0], nodes[3, 1] };
            nodes[3, 1].neighbors = new node[] { nodes[4, 1], nodes[3, 0] };
            nodes[3, 2].neighbors = new node[] { nodes[2, 2], nodes[4, 2] };
            nodes[3, 3].neighbors = new node[] { nodes[4, 3] };
            nodes[3, 4].neighbors = new node[] { nodes[2, 4], nodes[4, 4] };
            nodes[3, 5].neighbors = new node[] { nodes[2, 5], nodes[4, 5] };

            nodes[4, 0].neighbors = new node[] { nodes[5, 0], nodes[4, 1] };
            nodes[4, 1].neighbors = new node[] { nodes[3, 1], nodes[4, 0] };
            nodes[4, 2].neighbors = new node[] { nodes[3, 2], nodes[4, 3] };
            nodes[4, 3].neighbors = new node[] { nodes[4, 2], nodes[3, 3], nodes[5, 3] };
            nodes[4, 4].neighbors = new node[] { nodes[3, 4], nodes[5, 4] };
            nodes[4, 5].neighbors = new node[] { nodes[5, 5], nodes[3, 5] };

            nodes[5, 0].neighbors = new node[] { nodes[4, 0], nodes[5, 1] };
            nodes[5, 1].neighbors = new node[] { nodes[5, 0], nodes[5, 2] };
            nodes[5, 2].neighbors = new node[] { nodes[5, 1], nodes[5, 3] };
            nodes[5, 3].neighbors = new node[] { nodes[5, 2], nodes[4, 3] };
            nodes[5, 4].neighbors = new node[] { nodes[4, 4] };
            nodes[5, 5].neighbors = new node[] { nodes[4, 5] };

            nodes[3, 0].circle = true;
            nodes[2, 3].circle = true;

            return nodes;
        }

        private node[,] getMaze8()
        {
            node[,] nodes = createInitNodes();

            nodes[0, 0].neighbors = new node[] { nodes[0, 1] };
            nodes[0, 1].neighbors = new node[] { nodes[0, 0], nodes[0, 2] };
            nodes[0, 2].neighbors = new node[] { nodes[0, 1], nodes[0, 3], nodes[1, 2] };
            nodes[0, 3].neighbors = new node[] { nodes[0, 4], nodes[0, 2] };
            nodes[0, 4].neighbors = new node[] { nodes[0, 3], nodes[0, 5] };
            nodes[0, 5].neighbors = new node[] { nodes[0, 4], nodes[1, 5] };

            nodes[1, 0].neighbors = new node[] { nodes[1, 1], nodes[2, 0] };
            nodes[1, 1].neighbors = new node[] { nodes[1, 2], nodes[1, 0] };
            nodes[1, 2].neighbors = new node[] { nodes[0, 2], nodes[1, 1], nodes[2, 2] };
            nodes[1, 3].neighbors = new node[] { nodes[1, 4] };
            nodes[1, 4].neighbors = new node[] { nodes[1, 5], nodes[1, 3] };
            nodes[1, 5].neighbors = new node[] { nodes[0, 5], nodes[1, 4] };

            nodes[2, 0].neighbors = new node[] { nodes[3, 0], nodes[1, 0] };
            nodes[2, 1].neighbors = new node[] { nodes[3, 1], nodes[2, 2] };
            nodes[2, 2].neighbors = new node[] { nodes[2, 1], nodes[1, 2] };
            nodes[2, 3].neighbors = new node[] { nodes[3, 3], nodes[2, 4] };
            nodes[2, 4].neighbors = new node[] { nodes[2, 3], nodes[2, 5] };
            nodes[2, 5].neighbors = new node[] { nodes[2, 4], nodes[3, 5] };

            nodes[3, 0].neighbors = new node[] { nodes[2, 0], nodes[4, 0] };
            nodes[3, 1].neighbors = new node[] { nodes[2, 1] };
            nodes[3, 2].neighbors = new node[] { nodes[3, 3], nodes[4, 2] };
            nodes[3, 3].neighbors = new node[] { nodes[2, 3], nodes[3, 2] };
            nodes[3, 4].neighbors = new node[] { nodes[3, 5], nodes[4, 4] };
            nodes[3, 5].neighbors = new node[] { nodes[2, 5], nodes[3, 4] };

            nodes[4, 0].neighbors = new node[] { nodes[5, 0], nodes[4, 1], nodes[3, 0] };
            nodes[4, 1].neighbors = new node[] { nodes[4, 2], nodes[4, 0] };
            nodes[4, 2].neighbors = new node[] { nodes[3, 2], nodes[4, 1] };
            nodes[4, 3].neighbors = new node[] { nodes[5, 3] };
            nodes[4, 4].neighbors = new node[] { nodes[3, 4], nodes[4, 5] };
            nodes[4, 5].neighbors = new node[] { nodes[5, 5], nodes[4, 4] };

            nodes[5, 0].neighbors = new node[] { nodes[4, 0], nodes[5, 1] };
            nodes[5, 1].neighbors = new node[] { nodes[5, 0], nodes[5, 2] };
            nodes[5, 2].neighbors = new node[] { nodes[5, 1], nodes[5, 3] };
            nodes[5, 3].neighbors = new node[] { nodes[5, 2], nodes[4, 3], nodes[5, 4] };
            nodes[5, 4].neighbors = new node[] { nodes[5, 3] };
            nodes[5, 5].neighbors = new node[] { nodes[4, 5] };

            nodes[2, 1].circle = true;
            nodes[0, 4].circle = true;

            return nodes;
        }

        private node[,] createInitNodes()
        {
            node[,] nodes = new node[6,6];
            int count = 0;
            for (int x = 0; x < 6; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    nodes[x, y] = createNode(x, y, count);
                    count++;
                }

            }
            return nodes;
        }

        private node createNode(int x, int y, int id)
        {
            node node_ = new node();
            node_.id = id;
            node_.x = x;
            node_.y = y;
            node_.circle = false;
            return node_;
        }

    }
}
