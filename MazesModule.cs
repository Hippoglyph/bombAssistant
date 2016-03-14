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

        struct maze
        {
            public node[,] nodes;
        }

        struct node
        {
            public int x;
            public int y;
            public node[] neighbors;
            public bool circle;
            public bool player;
            public bool goal;
        }

        public MazesModule(Speaker talk, Listener rec)
        {
            this.talk = talk;
            this.rec = rec;
            exit = "Coordinates contained 0. Ready for new module";
            solve();
        }

        private void solve()
        {
            init();
        }

        private void init()
        {
            talk.speakAsync("What is the coordinates of one circle?");
            circle = rec.getCoords();
            if (hasZero(circle))
            {
                talk.speakAsync(exit);
                return;
            }
            talk.speakAsync("What is your position?");
            player = rec.getCoords();
            if (hasZero(player))
            {
                talk.speakAsync(exit);
                return;
            }
            talk.speakAsync("What is the position of the goal?");
            goal = rec.getCoords();
            if (hasZero(goal))
            {
                talk.speakAsync(exit);
                return;
            }
        }

        private bool hasZero(int[] coord)
        {
            if (coord[0] == 0 || coord[1] == 0)
                return true;
            return false;
        }

        private maze[] generateMazes()
        {
            maze[] mazes = new maze[9];
            mazes[0] = getMaze0();

            return mazes;
        }

        private maze getMaze0()
        {
            maze maze_ = new maze();
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

            return maze_;
        }

        private node[,] createInitNodes()
        {
            node[,] nodes = new node[6,6];
            for (int x = 0; x < 6; x++)
            {
                for (int y = 0; y < 6; y++)
                    nodes[x, y] = createNode(x, y);
            }
            return nodes;
        }

        private node createNode(int x, int y)
        {
            node node_ = new node();
            node_.x = x;
            node_.y = y;
            node_.player = false;
            node_.goal = false;
            node_.circle = false;
            return node_;
        }

    }
}
