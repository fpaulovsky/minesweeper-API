﻿using Microsoft.EntityFrameworkCore;
using MinesweeperAPI.Model;

namespace MinesweeperAPI.Data
{
    public class GameContext : DbContext
    {
        public GameContext(DbContextOptions<GameContext> options) : base(options)
        {

        }

        public DbSet<Game> Games { get; set; }

        public DbSet<Player> Players { get; set; }
    }
}
