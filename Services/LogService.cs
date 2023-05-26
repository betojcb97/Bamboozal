using Bamboo.DTO;
using Bamboo.Models;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Bamboo.Data;
using Microsoft.AspNetCore.Mvc;
using Azure.Core;
using Castle.Core.Smtp;
using System.Text.Encodings.Web;
using System;

namespace Bamboo.Services
{
    public class LogService : Controller
    {
        private BambooContext db;

        public LogService(BambooContext context)
        {
            db = context;
        }

        public void AddLog(string logInfo)
        {
            Log log = new Log();
            log.log = logInfo;
            db.Logs.Add(log);
            db.SaveChanges();
        }
    }
}
