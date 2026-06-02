# 레시피 목록

형식: [결과물 ID] 결과물이름 ← 재료1 x개 + 재료2 x개 + 재료3 x개
제작대 = Craft / 가마솥 = Cauldron

---

## 1단계 가공 재료 (Craft)
기본 자원 → 중간 재료. 다른 레시피의 재료로 사용됨.

| 결과물 ID | 결과물 이름 | 재료1 | 재료2 | 재료3 |
|-----------|------------|-------|-------|-------|
| mat_leather | 가죽 | animal_cow_hide x1 | stone_small x1 | |
| mat_rope | 밧줄 | animal_sheep_wool x2 | wood_oak x1 | |
| mat_bone_shard | 뼛조각 | animal_monkey_bone x1 | stone_small x1 | |
| mat_feather_bundle | 깃털 묶음 | animal_chick_feather x2 | animal_goose_feather x1 | |
| mat_sharp_fang | 날카로운 이빨 | animal_wolf_fang x1 | stone_small x1 | |
| mat_carved_bone | 조각된 뼈 | animal_gorilla_fang x1 | carved_stone x1 | |
| mat_poison_vial | 독액병 | animal_snake_venom x2 | item_rock_lava x1 | |
| mat_iron_ingot | 철 주괴 | item_ore_iron x2 | item_ore_coal x1 | |
| mat_gold_ingot | 금 주괴 | item_ore_gold x2 | item_ore_coal x1 | |
| mat_tanned_hide | 무두질 가죽 | mat_leather x1 | animal_horse_mane x1 | |
| mat_exotic_hide | 이국 가죽 | animal_zebra_hide x1 | animal_giraffe_hide x1 | |
| mat_ritual_feather | 의식 깃털 | mat_feather_bundle x1 | animal_rooster_redfeather x2 | |
| mat_ivory_shard | 상아 조각 | animal_elephant_tusk x1 | stone_small x2 | |
| mat_horn_powder | 뿔 가루 | animal_rhino_horn x1 | carved_stone x1 | |
| mat_acorn_oil | 도토리 기름 | animal_squirrel_acorn x3 | item_ore_coal x1 | |
| mat_fox_charm | 여우 부적 재료 | animal_fox_tail x1 | animal_squirrel_acorn x2 | |
| mat_husky_cloth | 북방 천 | animal_husky_fur x1 | animal_sheep_wool x1 | |
| mat_croc_scale | 악어 비늘 조각 | animal_crocodile_hide x1 | stone_small x1 | |
| mat_python_oil | 비단뱀 기름 | animal_python_skin x1 | item_ore_coal x1 | |
| mat_diamond_dust | 다이아 가루 | item_ore_diamond x1 | stone_small x2 | |

---

## 2단계 가공 재료 (Craft)
1단계 결과물 → 더 고급 재료.

| 결과물 ID | 결과물 이름 | 재료1 | 재료2 | 재료3 |
|-----------|------------|-------|-------|-------|
| mat_cursed_oil | 저주 기름 | mat_poison_vial x1 | mat_python_oil x1 | item_ore_ruby x1 |
| mat_sacred_cloth | 신성한 천 | mat_husky_cloth x1 | mat_ritual_feather x1 | |
| mat_war_powder | 전쟁 가루 | mat_horn_powder x1 | mat_iron_ingot x1 | |
| mat_sun_essence | 태양 정수 | mat_gold_ingot x1 | mat_ivory_shard x1 | item_ore_ruby x1 |
| mat_dark_essence | 어둠 정수 | mat_cursed_oil x1 | mat_carved_bone x1 | item_ore_coal x2 |
| mat_nature_essence | 자연 정수 | mat_acorn_oil x1 | mat_fox_charm x1 | crop_golden_berry x2 |

---

## 요리 - 1단계 (Cauldron)
기본 재료로 만드는 음식.

| 결과물 ID | 결과물 이름 | 재료1 | 재료2 | 재료3 |
|-----------|------------|-------|-------|-------|
| food_rabbit_roast | 토끼 구이 | animal_rabbit_meat x1 | wood_oak x1 | |
| food_hen_skewer | 닭꼬치 | animal_hen_meat x1 | wood_oak x1 | |
| food_pig_skewer | 돼지꼬치 | animal_pig_meat x1 | wood_oak x1 | |
| food_goat_skewer | 염소꼬치 | animal_goat_meat x1 | wood_oak x1 | |
| food_duck_skewer | 오리꼬치 | animal_duck_meat x1 | wood_oak x1 | |
| food_mushroom_skewer | 구운버섯꼬치 | crop_mushroom x2 | wood_oak x1 | |
| food_berry_salad | 베리샐러드 | crop_strawberry x1 | crop_blueberry x2 | |
| food_veggie_stew | 채소스튜 | crop_corn x1 | crop_potato x1 | crop_carrot x1 |
| food_tomato_soup | 토마토 수프 | crop_tomato x2 | crop_wheat x1 | |
| food_corn_bread | 옥수수빵 | crop_corn x2 | crop_wheat x1 | |
| food_potato_soup | 감자 수프 | crop_potato x2 | crop_carrot x1 | |
| food_acorn_porridge | 도토리죽 | animal_squirrel_acorn x3 | crop_wheat x1 | |

---

## 요리 - 2단계 (Cauldron)
1단계 요리 + 재료 조합.

| 결과물 ID | 결과물 이름 | 재료1 | 재료2 | 재료3 |
|-----------|------------|-------|-------|-------|
| food_meat_stew | 고기 스튜 | food_pig_skewer x1 | food_veggie_stew x1 | |
| food_roast_feast | 고기 성찬 | food_rabbit_roast x1 | food_hen_skewer x1 | crop_corn x1 |
| food_herb_soup | 약초 수프 | food_mushroom_skewer x1 | crop_carrot x1 | item_bush x2 |
| food_golden_porridge | 황금죽 | food_acorn_porridge x1 | crop_golden_berry x2 | |
| food_spicy_stew | 매운 스튜 | food_meat_stew x1 | crop_tomato x2 | |
| food_forest_soup | 숲의 수프 | food_veggie_stew x1 | crop_mushroom x2 | item_bush x1 |
| food_hunters_meal | 사냥꾼의 식사 | food_roast_feast x1 | food_herb_soup x1 | |

---

## 요리 - 3단계 (Cauldron)
최고급 요리. HP/Hunger 효과 높음.

| 결과물 ID | 결과물 이름 | 재료1 | 재료2 | 재료3 |
|-----------|------------|-------|-------|-------|
| food_golden_feast | 황금 만찬 | food_hunters_meal x1 | crop_golden_berry x3 | mat_acorn_oil x1 |
| food_sacred_broth | 신성한 국 | food_forest_soup x1 | mat_nature_essence x1 | crop_golden_berry x2 |
| food_warriors_feast | 전사의 만찬 | food_roast_feast x1 | food_spicy_stew x1 | mat_iron_ingot x1 |

---

## 제단 봉헌 아이템 - 1단계 토템 (Craft)
wood_totem + 동물 재료.

| 결과물 ID | 결과물 이름 | 재료1 | 재료2 | 재료3 |
|-----------|------------|-------|-------|-------|
| totem_wolf | 늑대 토템 | wood_totem x1 | animal_wolf_fang x2 | |
| totem_deer | 사슴 토템 | wood_totem x1 | animal_deer_antler x1 | |
| totem_bear | 곰 토템 | wood_totem x1 | animal_bear_claw x2 | |
| totem_fox | 여우 토템 | wood_totem x1 | animal_fox_tail x1 | animal_squirrel_acorn x2 |
| totem_eagle | 독수리 토템 | wood_totem x1 | mat_feather_bundle x1 | animal_rooster_redfeather x1 |
| totem_snake | 뱀 토템 | wood_totem x1 | animal_snake_venom x1 | animal_python_skin x1 |
| totem_reindeer | 순록 토템 | wood_totem x1 | animal_reindeer_antler x1 | |
| totem_moose | 무스 토템 | wood_totem x1 | animal_moose_antler x1 | hardwood x1 |
| totem_bull | 황소 토템 | wood_totem x1 | animal_bull_horn x2 | |
| totem_bison | 들소 토템 | wood_totem x1 | animal_bison_hide x1 | animal_bull_horn x1 |

---

## 제단 봉헌 아이템 - 2단계 조각상 (Craft)
carved_stone + 희귀 재료.

| 결과물 ID | 결과물 이름 | 재료1 | 재료2 | 재료3 |
|-----------|------------|-------|-------|-------|
| offering_ivory | 상아 조각상 | carved_stone x2 | mat_ivory_shard x1 | |
| offering_ruby_idol | 루비 신상 | carved_stone x2 | item_ore_ruby x2 | |
| offering_gold_idol | 황금 신상 | carved_stone x2 | mat_gold_ingot x2 | |
| offering_diamond_idol | 수정 신상 | carved_stone x2 | mat_diamond_dust x2 | |
| offering_bone_altar | 뼈 제단석 | carved_stone x1 | mat_carved_bone x2 | mat_bone_shard x2 |
| offering_croc_idol | 악어 신상 | carved_stone x2 | mat_croc_scale x2 | |
| offering_hippo_idol | 하마 신상 | carved_stone x2 | animal_hippo_tusk x2 | |

---

## 제단 봉헌 아이템 - 2단계 부적 (Craft)
의식용 부적류.

| 결과물 ID | 결과물 이름 | 재료1 | 재료2 | 재료3 |
|-----------|------------|-------|-------|-------|
| offering_ritual | 의식 부적 | mat_ritual_feather x1 | item_ore_ruby x1 | |
| offering_poison | 저주 부적 | mat_poison_vial x1 | item_ore_ruby x1 | |
| offering_fox_charm | 여우 부적 | mat_fox_charm x2 | item_ore_gold x1 | |
| offering_sun_charm | 태양 부적 | mat_ivory_shard x1 | mat_gold_ingot x1 | |
| offering_nature_charm | 자연 부적 | mat_nature_essence x1 | crop_golden_berry x2 | |
| offering_war_charm | 전쟁 부적 | mat_war_powder x1 | animal_lynx_claw x1 | |
| offering_shadow_charm | 어둠 부적 | mat_dark_essence x1 | animal_hedgehog_spine x2 | |
| offering_spine_fetish | 가시 부적 | animal_hedgehog_spine x3 | item_ore_ruby x1 | |

---

## 제단 봉헌 아이템 - 3단계 레전드 (Craft)
2단계 결과물 조합. 고점수 제출용.

| 결과물 ID | 결과물 이름 | 재료1 | 재료2 | 재료3 |
|-----------|------------|-------|-------|-------|
| legend_sun_idol | 태양 신상 | offering_gold_idol x1 | offering_sun_charm x1 | mat_sun_essence x1 |
| legend_war_totem | 부족장 토템 | totem_bull x1 | offering_ivory x1 | mat_war_powder x1 |
| legend_cursed_relic | 저주받은 유물 | offering_poison x1 | offering_shadow_charm x1 | mat_cursed_oil x1 |
| legend_ritual_crown | 의식 왕관 | offering_ritual x1 | mat_sacred_cloth x1 | mat_gold_ingot x2 |
| legend_lion_idol | 태양 사자상 | totem_wolf x1 | legend_sun_idol x1 | animal_lion_mane x1 |
| legend_nature_totem | 자연의 토템 | totem_deer x1 | totem_fox x1 | mat_nature_essence x1 |
| legend_shadow_totem | 어둠의 토템 | totem_snake x1 | legend_cursed_relic x1 | mat_dark_essence x1 |
| legend_ancestor_idol | 조상신 상 | offering_bone_altar x1 | offering_ivory x1 | mat_sacred_cloth x1 |
| legend_storm_totem | 폭풍 토템 | totem_moose x1 | totem_bison x1 | mat_war_powder x2 |
| legend_earth_idol | 대지 신상 | offering_hippo_idol x1 | offering_croc_idol x1 | mat_horn_powder x2 |

---

## ItemTable 추가분

```csv
mat_leather,가죽,Material,5
mat_rope,밧줄,Material,10
mat_bone_shard,뼛조각,Material,10
mat_feather_bundle,깃털 묶음,Material,10
mat_sharp_fang,날카로운 이빨,Material,5
mat_carved_bone,조각된 뼈,Material,5
mat_poison_vial,독액병,Material,5
mat_iron_ingot,철 주괴,Material,5
mat_gold_ingot,금 주괴,Material,5
mat_tanned_hide,무두질 가죽,Material,5
mat_exotic_hide,이국 가죽,Material,5
mat_ritual_feather,의식 깃털,Material,5
mat_ivory_shard,상아 조각,Material,5
mat_horn_powder,뿔 가루,Material,5
mat_acorn_oil,도토리 기름,Material,5
mat_fox_charm,여우 부적 재료,Material,5
mat_husky_cloth,북방 천,Material,5
mat_croc_scale,악어 비늘 조각,Material,5
mat_python_oil,비단뱀 기름,Material,5
mat_diamond_dust,다이아 가루,Material,5
mat_cursed_oil,저주 기름,Material,3
mat_sacred_cloth,신성한 천,Material,3
mat_war_powder,전쟁 가루,Material,3
mat_sun_essence,태양 정수,Material,3
mat_dark_essence,어둠 정수,Material,3
mat_nature_essence,자연 정수,Material,3
food_hen_skewer,닭꼬치,Food,5
food_pig_skewer,돼지꼬치,Food,5
food_goat_skewer,염소꼬치,Food,5
food_duck_skewer,오리꼬치,Food,5
food_mushroom_skewer,구운버섯꼬치,Food,5
food_berry_salad,베리샐러드,Food,5
food_veggie_stew,채소스튜,Food,5
food_tomato_soup,토마토 수프,Food,5
food_corn_bread,옥수수빵,Food,5
food_potato_soup,감자 수프,Food,5
food_acorn_porridge,도토리죽,Food,5
food_meat_stew,고기 스튜,Food,5
food_roast_feast,고기 성찬,Food,5
food_herb_soup,약초 수프,Food,5
food_golden_porridge,황금죽,Food,5
food_spicy_stew,매운 스튜,Food,5
food_forest_soup,숲의 수프,Food,5
food_hunters_meal,사냥꾼의 식사,Food,5
food_golden_feast,황금 만찬,Food,5
food_sacred_broth,신성한 국,Food,5
food_warriors_feast,전사의 만찬,Food,5
totem_wolf,늑대 토템,Material,1
totem_deer,사슴 토템,Material,1
totem_bear,곰 토템,Material,1
totem_fox,여우 토템,Material,1
totem_eagle,독수리 토템,Material,1
totem_snake,뱀 토템,Material,1
totem_reindeer,순록 토템,Material,1
totem_moose,무스 토템,Material,1
totem_bull,황소 토템,Material,1
totem_bison,들소 토템,Material,1
offering_ivory,상아 조각상,Material,1
offering_ruby_idol,루비 신상,Material,1
offering_gold_idol,황금 신상,Material,1
offering_diamond_idol,수정 신상,Material,1
offering_bone_altar,뼈 제단석,Material,1
offering_croc_idol,악어 신상,Material,1
offering_hippo_idol,하마 신상,Material,1
offering_ritual,의식 부적,Material,1
offering_poison,저주 부적,Material,1
offering_fox_charm,여우 부적,Material,1
offering_sun_charm,태양 부적,Material,1
offering_nature_charm,자연 부적,Material,1
offering_war_charm,전쟁 부적,Material,1
offering_shadow_charm,어둠 부적,Material,1
offering_spine_fetish,가시 부적,Material,1
legend_sun_idol,태양 신상,Material,1
legend_war_totem,부족장 토템,Material,1
legend_cursed_relic,저주받은 유물,Material,1
legend_ritual_crown,의식 왕관,Material,1
legend_lion_idol,태양 사자상,Material,1
legend_nature_totem,자연의 토템,Material,1
legend_shadow_totem,어둠의 토템,Material,1
legend_ancestor_idol,조상신 상,Material,1
legend_storm_totem,폭풍 토템,Material,1
legend_earth_idol,대지 신상,Material,1
```

## FoodTable 추가분

```csv
food_hen_skewer,닭꼬치,Hunger,25
food_pig_skewer,돼지꼬치,Hunger,25
food_goat_skewer,염소꼬치,Hunger,20
food_duck_skewer,오리꼬치,Hunger,20
food_mushroom_skewer,구운버섯꼬치,Hunger,20
food_berry_salad,베리샐러드,Hunger,25
food_veggie_stew,채소스튜,Hunger,35
food_tomato_soup,토마토 수프,Hunger,20
food_corn_bread,옥수수빵,Hunger,25
food_potato_soup,감자 수프,Hunger,30
food_acorn_porridge,도토리죽,Hunger,20
food_meat_stew,고기 스튜,Hunger,40
food_roast_feast,고기 성찬,Hunger,45
food_herb_soup,약초 수프,Hp,30
food_golden_porridge,황금죽,Hunger,35
food_spicy_stew,매운 스튜,Hunger,45
food_forest_soup,숲의 수프,Hunger,40
food_hunters_meal,사냥꾼의 식사,Hunger,55
food_golden_feast,황금 만찬,Hunger,70
food_sacred_broth,신성한 국,Hp,60
food_warriors_feast,전사의 만찬,Hunger,65
```
