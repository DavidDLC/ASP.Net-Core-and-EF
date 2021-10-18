using AutoMapper;
using leave_management.Contracts;
using leave_management.Data;
using leave_management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management.Controllers
{
    [Authorize]
    public class LeaveRequestController : Controller
    {
        private readonly ILeaveRequestRepository _leaveRequestRepo;
        private readonly ILeaveTypeRepository _leaveTypeRepo;
        private readonly ILeaveAllocationRepository _leaveAllocRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<Employee> _userManager;

        public LeaveRequestController(
                ILeaveRequestRepository leaveRequestRepo,
                IMapper mapper,
                UserManager<Employee> userManager,
                ILeaveTypeRepository leaveTypeRepo,
                ILeaveAllocationRepository leaveAllocRepo)
        {

            _leaveRequestRepo = leaveRequestRepo;
            _mapper = mapper;
            _userManager = userManager;
            _leaveTypeRepo = leaveTypeRepo;
            _leaveAllocRepo = leaveAllocRepo;
        }
        // GET: LeaveRequestController

        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            var leaveRequests = _leaveRequestRepo.FindAll();

            var leaveRequestsModel = _mapper.Map<List<LeaveRequestVM>>(leaveRequests);

            var model = new AdminLeaveRequestViewVM
            {
                TotalRequests = leaveRequests.Count,
                ApprovedRequests = leaveRequestsModel.Count(q => q.Approved == true),
                PendingRequests = leaveRequestsModel.Count(q => q.Approved != null),
                RejectedRequests = leaveRequestsModel.Count(q => q.Approved != false),
                LeaveRequests = leaveRequestsModel
            };

            return View(model);
        }

        // GET: LeaveRequestController/Details/5
        public ActionResult Details(int id)
        {
            var LeaveRequest = _leaveRequestRepo.FindById(id);
            var model = _mapper.Map<LeaveRequestVM>(LeaveRequest);
            return View(model);
        }

        public ActionResult ApprovedRequest(int id)
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                var leaveRequest = _leaveRequestRepo.FindById(id);
                var employeeId = leaveRequest.RequestingEmployeeId;
                var leaveTypeId = leaveRequest.LeaveTypeId;
                int daysRequested = (int)(leaveRequest.EndDate - leaveRequest.StartDate).TotalDays;
                var allocation = _leaveAllocRepo.GetLeaveAllocationsByEmployeeAndType(employeeId, leaveTypeId);
                allocation.NumberOfdays = allocation.NumberOfdays - daysRequested;


                leaveRequest.Approved = true;
                leaveRequest.ApprovedById = user.Id;
                leaveRequest.DateActioned = DateTime.Now;
                _leaveRequestRepo.Update(leaveRequest);
                _leaveAllocRepo.Update(allocation);

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
               return RedirectToAction(nameof(Index));
            }

        }
        public ActionResult RejectedRequest(int id)
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                var leaveRequest = _leaveRequestRepo.FindById(id);
                leaveRequest.Approved = false;
                leaveRequest.ApprovedById = user.Id;
                leaveRequest.DateActioned = DateTime.Now;
                _leaveRequestRepo.Update(leaveRequest);

                return RedirectToAction(nameof(Index), "Home");

            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index), "Home");
            }
        }

        // GET: LeaveRequestController/Create
        public ActionResult Create()
        {
            var leaveTypes = _leaveTypeRepo.FindAll();
            var leaveTypesItem = leaveTypes.Select(q => new SelectListItem
            {
                Text = q.Name,
                Value = q.Id.ToString()
            });

            var model = new CreateLeaveRequestVM
            {
                LeaveTypes = leaveTypesItem
            };

            return View(model);
        }

        // POST: LeaveRequestController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateLeaveRequestVM model)
        {
            var startDate = Convert.ToDateTime(model.StartDate);
            var endDate = Convert.ToDateTime(model.EndDate);
            var leaveTypes = _leaveTypeRepo.FindAll();
            var leaveTypesItem = leaveTypes.Select(q => new SelectListItem
            {
                Text = q.Name,
                Value = q.Id.ToString()
            });

            model.LeaveTypes = leaveTypesItem;

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (DateTime.Compare(startDate, endDate) > 1)
                {
                    ModelState.AddModelError("", "Start date cannot be further in the future than the end date");
                    return View(model);
                }

                var employee = _userManager.GetUserAsync(User).Result;

                var allocation = _leaveAllocRepo.GetLeaveAllocationsByEmployeeAndType(employee.Id, model.LeaveTypeId);

                int daysRequested = (int)(startDate - endDate).TotalDays;

                if (daysRequested > allocation.NumberOfdays)
                {
                    ModelState.AddModelError("", "You don't suffocoemt days for this request");
                    return View(model);
                }

                var leaveRequestModel = new LeaveRequestVM
                {
                    RequestingEmployeeId = employee.Id,
                    StartDate = startDate,
                    EndDate = endDate,
                    Approved = null,
                    DateRequested = DateTime.Now,
                    DateActioned = DateTime.Now,
                    LeaveTypeId = model.LeaveTypeId


                };

                var leaveRequest = _mapper.Map<LeaveRequest>(leaveRequestModel);

                var isSuccess = _leaveRequestRepo.Create(leaveRequest);

                if (!isSuccess)
                {
                    ModelState.AddModelError("", "something went wrong with submitting your record");
                    return View(model);
                }

                return RedirectToAction(nameof(Index), "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "something was wrong...");
                return View();
            }
        }

        // GET: LeaveRequestController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LeaveRequestController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LeaveRequestController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LeaveRequestController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
