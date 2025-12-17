// Copyright (C) 2025 FuseCP
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using FuseCP.EnterpriseServer.Data;

namespace FuseCP.EnterpriseServer
{
    public class TaskController: ControllerBase
    {
        public TaskController(ControllerBase provider) : base(provider) { }

        public BackgroundTask GetTask(string taskId)
        {
            BackgroundTask task = ObjectUtils.FillObjectFromDataReader<BackgroundTask>(
                Database.GetBackgroundTask(taskId));

            if (task == null)
            {
                return null;
            }

            task.Params = GetTaskParams(task.Id);

            return task;
        }

        public List<BackgroundTask> GetScheduleTasks(int scheduleId)
        {
            return ObjectUtils.CreateListFromDataReader<BackgroundTask>(
                Database.GetScheduleBackgroundTasks(scheduleId));
        }

        public List<BackgroundTask> GetTasks()
        {
            var user = SecurityContext.User;

            return GetTasks(user.IsPeer ? user.OwnerId : user.UserId);
        }

        public List<BackgroundTask> GetTasks(int actorId)
        {
            return ObjectUtils.CreateListFromDataReader<BackgroundTask>(
                Database.GetBackgroundTasks(actorId));
        }

        public List<BackgroundTask> GetTasks(Guid guid)
        {
            return ObjectUtils.CreateListFromDataReader<BackgroundTask>(
                Database.GetBackgroundTasks(guid));
        }

        public List<BackgroundTask> GetProcessTasks(BackgroundTaskStatus status)
        {
            return ObjectUtils.CreateListFromDataReader<BackgroundTask>(
                Database.GetProcessBackgroundTasks(status));
        }

        public BackgroundTask GetTopTask(Guid guid)
        {
            BackgroundTask task = ObjectUtils.FillObjectFromDataReader<BackgroundTask>(
                Database.GetBackgroundTopTask(guid));

            if (task == null)
            {
                return null;
            }

            task.Params = GetTaskParams(task.Id);

            return task;
        }

        public int AddTask(BackgroundTask task)
        {
            using (var clone = AsAsync<TaskController>())
            {
                int taskId = clone.Database.AddBackgroundTask(task.Guid, task.TaskId, task.ScheduleId, task.PackageId, task.UserId,
                                                            task.EffectiveUserId, task.TaskName, task.ItemId, task.ItemName,
                                                            task.StartDate, task.IndicatorCurrent, task.IndicatorMaximum,
                                                            task.MaximumExecutionTime, task.Source, task.Severity, task.Completed,
                                                            task.NotifyOnComplete, task.Status);

                clone.AddTaskParams(taskId, task.Params);

                clone.Database.AddBackgroundTaskStack(taskId);
			
                return taskId;
			}
		}

        public void UpdateTaskWithParams(BackgroundTask task)
        {
            if (UpdateTask(task))
            {
                UpdateBackgroundTaskParams(task);
            }
        }

        public bool UpdateTask(BackgroundTask task)
        {
            if (task.Status == BackgroundTaskStatus.Abort)
            {
                DeleteBackgroundTasks(task.Guid);

                return false;
            }

            if (task.Completed)
            {
                DeleteBackgroundTask(task.Id);

                return false;
            }

            using (var db = Database.Context)
            {
                db.UpdateBackgroundTask(task.Guid, task.Id, task.ScheduleId, task.PackageId, task.TaskName, task.ItemId,
                                              task.ItemName, task.FinishDate, task.IndicatorCurrent,
                                              task.IndicatorMaximum, task.MaximumExecutionTime, task.Source,
                                              task.Severity, task.Completed, task.NotifyOnComplete, task.Status);
            }
            return true;
        }

        public void UpdateBackgroundTaskParams(BackgroundTask task)
        {
            Database.DeleteBackgroundTaskParams(task.Id);

            AddTaskParams(task.Id, task.Params);
        }

        public void DeleteBackgroundTasks(Guid guid)
        {
            Database.DeleteBackgroundTasks(guid);
        }

        public void DeleteBackgroundTask(int id)
        {
            Database.DeleteBackgroundTask(id);
        }

        public void AddTaskParams(int taskId, List<BackgroundTaskParameter> parameters)
        {
            using (var db = Database.Context)
            {
                foreach (BackgroundTaskParameter param in SerializeParams(parameters))
                {
                    db.AddBackgroundTaskParam(taskId, param.Name, param.SerializerValue, param.TypeName);
                }
            }
        }

        public List<BackgroundTaskParameter> GetTaskParams(int taskId)
        {
            List<BackgroundTaskParameter> parameters = ObjectUtils.CreateListFromDataReader<BackgroundTaskParameter>(
                Database.GetBackgroundTaskParams(taskId));

            return DeserializeParams(parameters);
        }

        public void AddLog(BackgroundTaskLogRecord log)
        {
            using (var db = Database.Context)
            {
                db.AddBackgroundTaskLog(log.TaskId, log.Date, log.ExceptionStackTrace, log.InnerTaskStart,
                                              log.Severity, log.Text, log.TextIdent, BuildParametersXml(log.TextParameters));
            }
        }

        public List<BackgroundTaskLogRecord> GetLogs(BackgroundTask task, DateTime startLogTime)
        {
            if (startLogTime <= task.StartDate)
            {
                startLogTime = task.StartDate;
            }

            List<BackgroundTaskLogRecord> logs = ObjectUtils.CreateListFromDataReader<BackgroundTaskLogRecord>(
                Database.GetBackgroundTaskLogs(task.Id, startLogTime));

            foreach (BackgroundTaskLogRecord log in logs)
            {
                log.TextParameters = ReBuildParametersXml(log.XmlParameters);
            }

            return logs;
        }

        private List<BackgroundTaskParameter> SerializeParams(List<BackgroundTaskParameter> parameters)
        {
            foreach (BackgroundTaskParameter param in parameters)
            {
                var type = param.Value.GetType();
                param.TypeName = type.FullName;

                XmlSerializer serializer = new XmlSerializer(type);
                MemoryStream ms = new MemoryStream();
                serializer.Serialize(ms, param.Value);

                ms.Position = 0;
                StreamReader sr = new StreamReader(ms);

                param.SerializerValue = sr.ReadToEnd();
            }

            return parameters;
        }

        private List<BackgroundTaskParameter> DeserializeParams(List<BackgroundTaskParameter> parameters)
        {
            foreach (BackgroundTaskParameter param in parameters)
            {
                XmlSerializer deserializer = new XmlSerializer(Type.GetType(param.TypeName));
                StringReader sr = new StringReader(param.SerializerValue);

                param.Value = deserializer.Deserialize(sr);
            }

            return parameters;
        }

        private string BuildParametersXml(string[] parameters)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement nodeProps = xmlDoc.CreateElement("parameters");

            if (parameters != null)
            {
                foreach (string parameter in parameters)
                {
                    XmlElement nodeProp = xmlDoc.CreateElement("parameter");
                    nodeProp.SetAttribute("value", parameter);
                    nodeProps.AppendChild(nodeProp);
                }
            }
            return nodeProps.OuterXml;
        }

        private string[] ReBuildParametersXml(string parameters)
        {
            var textParameters = new List<string>();

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(parameters);

            if (xmlDoc != null)
            {
                textParameters.AddRange(from XmlNode xmlParameter in xmlDoc.SelectNodes("parameters/parameter") select xmlParameter.Attributes.GetNamedItem("value").Value);
            }

            return textParameters.ToArray();
        }
    }
}

