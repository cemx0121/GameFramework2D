using GameFramework2D.Models;
using GameFramework2D.WorldObjectFactory;
using System.Diagnostics;

// Adding the ConsoleTraceListener
Trace.Listeners.Add(new ConsoleTraceListener());

// Create a world with a size of 10x10
World world = new World(10, 10);

// Create a world object factory
WorldObjectFactory factory = new DefaultWorldObjectFactory();

// Create creatures
Creature creature1 = factory.CreateCreature(5, 5, "Cem", 100);
Creature creature2 = factory.CreateCreature(4, 4, "Amani", 50);
Creature creature3 = factory.CreateCreature(1, 2, "Liyana", 50);
Creature creature4 = factory.CreateCreature(7, 4, "Bilo", 50);

// Add creatures to the world
world.AddCreature(creature1);
world.AddCreature(creature2);
world.AddCreature(creature3);
world.AddCreature(creature4);

// Create world objects
AttackItem attackItem = factory.CreateAttackItem(5, 4, true, 100, "Great Wand", "A grey wand with much power");
AttackItem attackItem1 = factory.CreateAttackItem(4, 3, true, 100, "Great Wand", "A grey wand with much power");
AttackItem attackItem2 = factory.CreateAttackItem(1, 1, true, 100, "Great Wand", "A grey wand with much power");
AttackItem attackItem3 = factory.CreateAttackItem(7, 3, true, 100, "Great Wand", "A grey wand with much power");
DefenceItem defenceItem = factory.CreateDefenceItem(5, 2, true, 5, "Magic shield", "An old weak shield");
Obstacle Tree = factory.CreateObstacle(5, 6, "Tall Tree", "Oak tree blocking the view");
Obstacle Rock = factory.CreateObstacle(5, 3, "Big Rock", "Round rock blocking the path");
// Add world objects to the world
world.AddWorldObject(attackItem);
world.AddWorldObject(attackItem1);
world.AddWorldObject(attackItem2);
world.AddWorldObject(attackItem3);
world.AddWorldObject(defenceItem);
world.AddWorldObject(Tree);
world.AddWorldObject(Rock);
world.GetWorldStatus();

world.GameLoop(50);

world.GetWorldStatus();
