using AutoMapper;
using BulkyBook.BusinessCore.Services;
using BulkyBook.Presentation;
using BulkyBook.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BulkyBook.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class CoverTypesController : Controller
{
    private readonly ICoverTypesService _coverTypesService;
    private readonly IMapper _mapper;
    private readonly ILogger<CoverTypesController> _logger;

    public CoverTypesController(ICoverTypesService coverTypesService
        , IMapper mapper
        , ILogger<CoverTypesController> logger)
    {
        _coverTypesService = coverTypesService;
        _mapper = mapper;
        _logger = logger;
    }

    // GET /Admin/CoverTypes/Index
    public async Task<IActionResult> Index()
    {
        var coverTypes = await _coverTypesService.GetAllCoverTypesAsync();
        return View(coverTypes);
    }

    // GET /Admin/CoverTypes/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST /Admin/CoverTypes/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCoverTypeViewModel createCoverType)
    {
        if (!ModelState.IsValid)
        {
            return View(createCoverType);
        }

        var created = await _coverTypesService.CreateNewCoverTypeAsync(createCoverType);
        if (created.IsFailure)
        {
            ModelState.AddModelError(nameof(CreateCoverTypeViewModel.Name)
                , created.ErrorMessages[0]);
            return View(createCoverType);
        }

        TempData[TempDataKeys.SuccessMessage] =
            $"Cover Type [{created.Value.Name}] has been created.";
        return RedirectToAction(nameof(Index));
    }

    // GET /Admin/CoverTypes/Edit/{id}
    public async Task<IActionResult> Edit(Guid id)
    {
        var response = await _coverTypesService.GetSingleCoverTypeByIdAsync(id);
        if (response.IsFailure)
        {
            TempData[TempDataKeys.FailureMessage] = $"Cover Type with id:{id} is not found.";
            return RedirectToAction(nameof(Index));
        }

        return View(_mapper.Map<EditCoverTypeViewModel>(response.Value));
    }

    // POST /Admin/CoverTypes/Edit/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, EditCoverTypeViewModel editCoverType)
    {
        if (!ModelState.IsValid)
        {
            return View(editCoverType);
        }

        var updated = await _coverTypesService.UpdateCoverTypeAsync(editCoverType);
        if (updated.IsFailure)
        {
            ModelState.AddModelError(nameof(EditCoverTypeViewModel.Name)
                , updated.ErrorMessages[0]);
            return View(editCoverType);
        }

        TempData[TempDataKeys.SuccessMessage] =
            $"Cover Type [{updated.Value.Name}] has been updated.";
        return RedirectToAction(nameof(Index));
    }

    // GET /Admin/CoverTypes/Delete/{id}
    public async Task<IActionResult> Delete(Guid id)
    {
        var coverType = await _coverTypesService.GetSingleCoverTypeByIdAsync(id);
        if (coverType.IsFailure)
        {
            TempData[TempDataKeys.FailureMessage] = $"Cover Type with id:{id} is not found.";
            return RedirectToAction(nameof(Index));
        }

        return View(coverType.Value);
    }

    // POST /Admin/CoverTypes/Delete/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, CoverTypeViewModel deleteCoverType)
    {
        if (!ModelState.IsValid)
        {
            return View(deleteCoverType);
        }

        var coverType = await _coverTypesService.GetSingleCoverTypeByIdAsync(deleteCoverType.Id);
        if (coverType.IsFailure)
        {
            TempData[TempDataKeys.FailureMessage] = $"Cover Type with id:{id} is not found.";
            return RedirectToAction(nameof(Index));
        }

        var response = await _coverTypesService.DeleteCoverTypeByIdAsync(deleteCoverType.Id);
        if (response.IsFailure)
        {
            TempData[TempDataKeys.FailureMessage] = response.ErrorMessages[0];
            return RedirectToAction(nameof(Index));
        }

        TempData[TempDataKeys.SuccessMessage] =
            $"Cover Type [{deleteCoverType.Name}] has been deleted.";
        return RedirectToAction(nameof(Index));
    }
}