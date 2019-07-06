using System.Linq;
using System.Threading.Tasks;
using Alsein.Extensions;

namespace Cynthia.Card
{
    [CardEffectId("22014")]//煮婆：仪式
    public class BrewessRitual : CardEffect
    {//复活2个铜色遗愿单位。
        public BrewessRitual(GameCard card) : base(card){ }
        public override async Task<int> CardPlayEffect(bool isSpying,bool isReveal)
        {
            var list = Game.PlayersCemetery[PlayerIndex]
                .Where((x.HideTags == null ? false : x.HideTags.Contains(HideTag.Deathwish))
                    && x.Status.Group == Group.Copper
                    && x.CardInfo().CardType == CardType.Unit).ToList();

            //让玩家选择两张铜色遗愿单位
            if (list.Count() <= 0)
            {
                return 0;
            }
            else if (list.Count() == 1)
            {
                var result = await Game.GetSelectMenuCards(Card.PlayerIndex, list, 1, isCanOver:true);
            }
            else
            {
                var result = await Game.GetSelectMenuCards(Card.PlayerIndex, list, 2, isCanOver:true);
            }
            result = result.ToList();

            //如果玩家一张卡都没选择,没有效果
            if (result.Count() == 0)
            {
                return 0;
            }

            //复活铜色遗愿单位
            foreach(var card in result)
            {
                await card.Effect.Resurrect(new CardLocation() { RowPosition = RowPosition.MyStay, CardIndex = 0 }, Card);
            }

            return result.Count();
        }
    }
}