(() => {
  "use strict";

  const events = Array.isArray(riftorynTimeline) ? riftorynTimeline : [];
  const heroMeta = Array.isArray(riftorynTimelineHeroes) ? riftorynTimelineHeroes : [];
  const metaById = new Map(heroMeta.map(hero => [hero.id, hero]));
  const eraTabs = document.getElementById("era-tabs");
  const heroFilter = document.getElementById("hero-filter");
  const yearJump = document.getElementById("year-jump");
  const timelineList = document.getElementById("timeline-list");
  const searchInput = document.getElementById("timeline-search");
  const eventCount = document.getElementById("event-count");
  let era = "all";
  let selectedHero = null;

  function element(tag, text, className) {
    const node = document.createElement(tag);
    if (text) node.textContent = text;
    if (className) node.className = className;
    return node;
  }

  function timelineEra(event) {
    if (event.era === "PT") return "PT";
    return "ET";
  }

  function eraLabel(event) {
    if (event.kind === "history") return event.year || "Historia świata";
    const match = event.date.match(/(?:^|[,\s])(\d+)\s+(PT|ET)\b/);
    if (match) return `${match[1]} ${match[2]}`;
    if (/3\s*[–-]\s*2\s+PT/.test(event.date)) return "Wojna i oblężenie";
    return event.year || "Bez roku";
  }

  function plural(count, forms) {
    const lastTwo = count % 100;
    const last = count % 10;
    if (count === 1) return forms[0];
    if (last >= 2 && last <= 4 && (lastTwo < 12 || lastTwo > 14)) return forms[1];
    return forms[2];
  }

  function setupEraTabs() {
    const eras = [
      ["all", "Wszystkie"],
      ["PT", "Przed Traktatem"],
      ["ET", "Era Traktatu"],
    ];
    eraTabs.replaceChildren();
    eras.forEach(([id, label]) => {
      const button = element("button", label, "era-tab");
      button.type = "button";
      button.role = "tab";
      button.setAttribute("aria-selected", String(era === id));
      button.addEventListener("click", () => {
        era = id;
        setupEraTabs();
        render();
      });
      eraTabs.append(button);
    });
  }

  function setupHeroFilter() {
    const available = new Map();
    events.forEach(event => event.heroes.forEach(id => available.set(id, metaById.get(id))));
    const heroes = Array.from(available.keys())
      .map(id => metaById.get(id) || {id, name: id, portrait: null})
      .sort((a, b) => a.name.localeCompare(b.name, "pl"));

    heroFilter.replaceChildren();
    const all = element("button");
    all.type = "button";
    all.setAttribute("aria-pressed", String(selectedHero === null));
    all.append(element("span", null, "filter-placeholder"));
    all.querySelector(".filter-placeholder").textContent = "✦";
    all.append(element("span", "Wszyscy bohaterowie"));
    all.addEventListener("click", () => { selectedHero = null; setupHeroFilter(); render(); });
    heroFilter.append(all);

    heroes.forEach(hero => {
      const button = element("button");
      button.type = "button";
      button.setAttribute("aria-pressed", String(selectedHero === hero.id));
      if (hero.portrait) {
        const image = element("img");
        image.src = hero.portrait;
        image.alt = "";
        image.loading = "lazy";
        button.append(image);
      } else {
        const placeholder = element("span", hero.name.slice(0, 1), "filter-placeholder");
        button.append(placeholder);
      }
      button.append(element("span", hero.name));
      button.addEventListener("click", () => {
        selectedHero = selectedHero === hero.id ? null : hero.id;
        setupHeroFilter();
        render();
      });
      heroFilter.append(button);
    });
  }

  function matches(event) {
    if (era !== "all" && timelineEra(event) !== era) return false;
    if (selectedHero && event.heroes.length && !event.heroes.includes(selectedHero)) return false;
    const query = searchInput.value.trim().toLocaleLowerCase("pl");
    if (!query) return true;
    const names = event.heroes.map(id => metaById.get(id)?.name || id).join(" ");
    return `${event.date} ${event.location} ${event.title} ${event.description || ""} ${names}`
      .toLocaleLowerCase("pl")
      .includes(query);
  }

  function render() {
    const filtered = events.filter(matches);
    eventCount.textContent = filtered.length.toLocaleString("pl");

    const groups = new Map();
    filtered.forEach(event => {
      const label = eraLabel(event);
      if (!groups.has(label)) groups.set(label, []);
      groups.get(label).push(event);
    });

    yearJump.replaceChildren();
    timelineList.replaceChildren();
    if (!filtered.length) {
      timelineList.append(element("p", "Brak wpisów spełniających wybrane kryteria.", "empty-timeline"));
      return;
    }

    Array.from(groups.entries()).forEach(([yearLabel, yearEvents]) => {
      const id = `rok-${encodeURIComponent(yearLabel).replace(/%/g, "-")}`;
      const jump = element("a", yearLabel);
      jump.href = `#${id}`;
      yearJump.append(jump);

      const group = element("section", null, "year-group");
      group.id = id;
      const heading = element("h2", null, "year-heading");
      const historyCount = yearEvents.filter(event => event.kind === "history").length;
      const storyCount = yearEvents.length - historyCount;
      const countLabels = [];
      if (historyCount) countLabels.push(`${historyCount} ${plural(historyCount, ["wydarzenie", "wydarzenia", "wydarzeń"])}`);
      if (storyCount) countLabels.push(`${storyCount} ${plural(storyCount, ["scena", "sceny", "scen"])}`);
      heading.append(element("strong", yearLabel), element("span", countLabels.join(" · ")));
      group.append(heading);

      yearEvents.forEach(event => {
        const row = element("article", null, "timeline-event");
        const date = element("div", event.datePart || event.date, "event-date");
        const card = element("div", null, "event-card");
        card.append(element("p", event.location || "Miejsce niepodane", "event-location"));
        if (event.kind === "history") {
          card.append(element("p", "Wydarzenie historyczne", "event-kind"));
        }
        card.append(element("h3", event.title));
        if (event.description) {
          card.append(element("p", event.description, "event-description"));
        }

        const heroes = element("div", null, "event-heroes");
        event.heroes.forEach(id => {
          const hero = metaById.get(id) || {id, name: id, portrait: null};
          const item = element("span", null, "event-hero");
          if (hero.portrait) {
            const image = element("img");
            image.src = hero.portrait;
            image.alt = "";
            image.loading = "lazy";
            item.append(image);
          }
          item.append(document.createTextNode(hero.name));
          heroes.append(item);
        });

        let source = null;
        if (event.link) {
          source = element("a", "Czytaj scenę →", "event-source");
          source.href = `archiwum-riftoryn.html${event.link}`;
        } else if (event.sourceLink) {
          source = element("a", "Czytaj kanon świata →", "event-source");
          source.href = event.sourceLink;
        }
        if (event.heroes.length || source) {
          if (source) heroes.append(source);
          card.append(heroes);
        }
        row.append(date, card);
        group.append(row);
      });
      timelineList.append(group);
    });
  }

  searchInput.addEventListener("input", render);
  setupEraTabs();
  setupHeroFilter();
  render();
})();
