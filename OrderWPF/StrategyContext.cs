using Newtonsoft.Json;
using OrderWPF.AIStrategy;
using OrderWPF.AItool;
using OrderWPF.DiscountStrategies;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderWPF
{
    internal class StrategyContext
    {
        OrderRequestModel _orderRequestModel;
        RenderModel _renderModel;
        public StrategyContext(OrderRequestModel orderRequestModel)
        {
            _orderRequestModel = orderRequestModel;
        }
        public async Task<RenderModel> GetCuponStrategyAsync()
        {
            _orderRequestModel.Orders.RemoveAll(x => x.Name.Contains("折扣"));
            _orderRequestModel.Orders.RemoveAll(x => x.Name.Contains("贈送"));
            _orderRequestModel.Orders.RemoveAll(x => x.Name.Contains("優惠"));

            if (!_orderRequestModel.EnableAISuggestion && _orderRequestModel.DiscountType != null)
            {
                Type discounttype = Type.GetType(_orderRequestModel.DiscountType.DiscountType);
                CuponStrategy strategy = (CuponStrategy)Activator.CreateInstance(discounttype, _orderRequestModel.Orders, _orderRequestModel.DiscountType);
                strategy.ApplyCupon();

                _renderModel = new RenderModel(_orderRequestModel.Orders, null);
                return _renderModel;

            }
            else if(_orderRequestModel.EnableAISuggestion)
            {
                AIagent aIagent = new AIagent();

                string menuText = AppData.MenuData;

                aIagent.AddPrompt($"以下是我的菜單以及折扣的策略:{menuText}");
                aIagent.AddPrompt($"以下是我所購買的品項:" + String.Join("\r\n", _orderRequestModel.Orders.Select(x => $"{x.Name}:{x.Count}份")));
                aIagent.AddPrompt("如果發現有多種可能的折扣優惠，請不要組合折扣，請給我最優惠的其中一種折扣策略");
                
                AIResult aIResult = await aIagent.GetResult();

                if (!aIResult.CanExcuteTool)
                {
                    Console.WriteLine(aIResult.ResponseText);
                }
                else
                {
                    string response = JsonConvert.SerializeObject(aIResult.Runtool());
                    
                    DiscountStrategyModel responseModel = JsonConvert.DeserializeObject<DiscountStrategyModel>(response);
                                                                
                    OrderWPF.Models.MenuModel.Discount discountType = AppData.Discounts.First(x => x.DiscountType == responseModel.discountType && x.Type == responseModel.discountName);
                    Type discounttype = Type.GetType(responseModel.discountType);
                    CuponStrategy strategy = (CuponStrategy)Activator.CreateInstance(discounttype, _orderRequestModel.Orders, discountType);
                    strategy.ApplyCupon();
                    _renderModel = new RenderModel(_orderRequestModel.Orders, responseModel.reason);
                }               
                return _renderModel;
            }
            return _renderModel;
        }
    }
}
