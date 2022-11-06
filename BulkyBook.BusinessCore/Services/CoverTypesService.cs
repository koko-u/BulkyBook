using AutoMapper;
using BulkyBook.Persistence.Data;
using BulkyBook.Persistence.Models;
using BulkyBook.Presentation.Result;
using BulkyBook.Presentation.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BulkyBook.BusinessCore.Services;

public class CoverTypesService : ICoverTypesService
{
    private readonly BulkyBookDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<CoverTypesService> _logger;

    public CoverTypesService(BulkyBookDbContext dbContext
        , IMapper mapper
        , ILogger<CoverTypesService> logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<CoverTypeViewModel>> GetAllCoverTypesAsync()
    {
        var coverTypes = await _dbContext.CoverTypes.ToListAsync();

        return _mapper.Map<IEnumerable<CoverTypeViewModel>>(coverTypes);
    }

    public async Task<ResponseData<CoverTypeViewModel>> CreateNewCoverTypeAsync(
        CreateCoverTypeViewModel createCoverType)
    {
        var coverType = _mapper.Map<CoverType>(createCoverType);
        try
        {
            await _dbContext.AddAsync(coverType);
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e.Message);
            _logger.LogError(e.StackTrace);

            return ResponseData.Error<CoverTypeViewModel>(e.Message);
        }

        return ResponseData.Ok(_mapper.Map<CoverTypeViewModel>(coverType));
    }

    public async Task<ResponseData<CoverTypeViewModel>> GetSingleCoverTypeByIdAsync(Guid id)
    {
        var coverType = await _dbContext.CoverTypes.FindAsync(id);
        if (coverType == null)
        {
            return ResponseData.Error<CoverTypeViewModel>($"Cover Type of id;{id} is not found");
        }

        return ResponseData.Ok(_mapper.Map<CoverTypeViewModel>(coverType));
    }

    public async Task<ResponseData<CoverTypeViewModel>> UpdateCoverTypeAsync(
        EditCoverTypeViewModel editCoverType)
    {
        var target = await _dbContext.CoverTypes.FindAsync(editCoverType.Id);
        if (target == null)
        {
            return ResponseData.Error<CoverTypeViewModel>(
                $"Cover Type of id;{editCoverType.Id} is not found");
        }

        _mapper.Map(editCoverType, target);
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e.Message);
            _logger.LogError(e.StackTrace);

            return ResponseData.Error<CoverTypeViewModel>(e.Message);
        }

        return ResponseData.Ok(_mapper.Map<CoverTypeViewModel>(target));
    }

    public async Task<ResponseData> DeleteCoverTypeByIdAsync(Guid id)
    {
        var target = await _dbContext.CoverTypes.FindAsync(id);
        if (target == null)
        {
            return ResponseData.Error($"Cover Type of id;{id} is not found");
        }

        _dbContext.CoverTypes.Remove(target);
        await _dbContext.SaveChangesAsync();

        return ResponseData.Ok();
    }
}