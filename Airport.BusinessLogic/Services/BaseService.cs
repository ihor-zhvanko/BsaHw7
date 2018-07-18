using System;
using System.Collections.Generic;
using Airport.Data.UnitOfWork;
using Airport.Data.Models;
using AutoMapper;
using FluentValidation;

using Airport.Common.Exceptions;
using System.Threading.Tasks;

namespace Airport.BusinessLogic.Services
{
  public class BaseService<TDTO, TEntity> : IService<TDTO> where TEntity : Entity
  {
    protected IUnitOfWork _unitOfWork;
    protected IValidator<TDTO> _dtoValidator;

    public BaseService(IUnitOfWork unitOfWork, IValidator<TDTO> dtoValidator)
    {
      _unitOfWork = unitOfWork;
      _dtoValidator = dtoValidator;
    }

    protected async Task EnsureValidAsync(TDTO model)
    {
      var validationResult = await _dtoValidator.ValidateAsync(model);
      if (!validationResult.IsValid)
      {
        throw new BadRequestException(validationResult.Errors);
      }
    }

    public async Task<TDTO> CreateAsync(TDTO model)
    {
      await EnsureValidAsync(model);

      var entity = Mapper.Map<TEntity>(model);
      entity = await _unitOfWork.Set<TEntity>().CreateAsync(entity);
      await _unitOfWork.SaveChangesAsync();

      return Mapper.Map<TDTO>(entity);
    }

    public virtual async Task DeleteAsync(TDTO model)
    {
      var entity = Mapper.Map<TEntity>(model);
      _unitOfWork.Set<TEntity>().Delete(entity);
      await _unitOfWork.SaveChangesAsync();
    }

    public virtual async Task Delete(int id)
    {
      await _unitOfWork.Set<TEntity>().DeleteAsync(id);
      await _unitOfWork.SaveChangesAsync();
    }

    public virtual async Task<IList<TDTO>> GetAll()
    {
      var entities = await _unitOfWork.Set<TEntity>().Get().ToListAsync();
      return Mapper.Map<IList<TDTO>>(entities);
    }

    public virtual TDTO GetById(int id)
    {
      var entity = _unitOfWork.Set<TEntity>().Get(id);
      if (entity == null)
        throw new NotFoundException(typeof(TEntity).Name + " with such id was not found");

      return Mapper.Map<TDTO>(entity);
    }

    public virtual TDTO Update(TDTO model)
    {
      EnsureValid(model);

      var entity = Mapper.Map<TEntity>(model);
      entity = _unitOfWork.Set<TEntity>().Update(entity);
      _unitOfWork.SaveChanges();

      return Mapper.Map<TDTO>(entity);
    }
  }
}