using System;
using System.Collections.Generic;
using Airport.Data.UnitOfWork;
using Airport.Data.Models;
using AutoMapper;
using FluentValidation;

using Airport.Common.Exceptions;

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

    protected void EnsureValid(TDTO model)
    {
      var validationResult = _dtoValidator.Validate(model);
      if (!validationResult.IsValid)
      {
        throw new BadRequestException(validationResult.Errors);
      }
    }

    public TDTO Create(TDTO model)
    {
      EnsureValid(model);

      var entity = Mapper.Map<TEntity>(model);
      entity = _unitOfWork.Set<TEntity>().Create(entity);
      _unitOfWork.SaveChanges();

      return Mapper.Map<TDTO>(entity);
    }

    public virtual void Delete(TDTO model)
    {
      var entity = Mapper.Map<TEntity>(model);
      _unitOfWork.Set<TEntity>().Delete(entity);
      _unitOfWork.SaveChanges();
    }

    public virtual void Delete(int id)
    {
      _unitOfWork.Set<TEntity>().Delete(id);
      _unitOfWork.SaveChanges();
    }

    public virtual IList<TDTO> GetAll()
    {
      var entities = _unitOfWork.Set<TEntity>().Get();
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