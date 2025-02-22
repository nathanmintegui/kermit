<script lang="ts">
    import type { PageProps } from './$types';

    let { data }: PageProps = $props();

    let competencias = data?.data?.competencias;

    const formatDate = (date) => date.split("/")[0];
</script>

<div class="text-center">
     <h1 class="display-4">Calendário Geral</h1>

    <div class="calendar-container">
        {#each competencias as competencia}
            <div class="b-0">
                <div class="calendar">
                    <div class="month-indicator">{competencia?.mes}</div>
                    <div class="day-of-week">
                        <div>Dom</div>
                        <div>Seg</div>
                        <div>Ter</div>
                        <div>Qua</div>
                        <div>Qui</div>
                        <div>Sex</div>
                        <div>Sáb</div>
                    </div>
                    <div class="date-grid">
                        {#each competencia?.dias as dia}
                            {#if dia?.data == ""}
                                <button>
                                    <span>&nbsp</span>
                                </button>
                            {:else}
                                <button>
                                    <time datetime="{dia?.data}">{formatDate(dia?.data)}</time>
                                </button>
                            {/if}
                        {/each}
                    </div>
                </div>
            </div>
        {/each}
    </div>
</div>

<style>
* {
        cursor: default;
    }

    .calendar-container {
        display: flex;
        gap: 3em;
        flex-wrap: wrap;
    }

    .calendar {
        max-width: 400px;
        margin: 20px auto;
        padding: 15px;
        border-radius: 8px;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        background-color: #ffffff;
    }

    .month-indicator {
        text-align: center;
        font-size: 1.5rem;
        font-weight: bold;
        margin-bottom: 10px;
        color: #333;
    }

    .day-of-week {
        display: grid;
        grid-template-columns: repeat(7, 1fr);
        text-align: center;
        font-weight: bold;
        color: #555;
        padding: 5px 0;
        background-color: #f8f9fa;
        border-radius: 6px 6px 0 0;
    }

    .date-grid {
        display: grid;
        grid-template-columns: repeat(7, 1fr);
        gap: 5px;
        padding: 10px;
        background-color: #fdfdfd;
        border-radius: 0 0 6px 6px;
    }

    button {
        width: 100%;
        padding: 10px;
        text-align: center;
        background-color: #ffffff;
        border: 1px solid #ddd;
        border-radius: 4px;
        cursor: pointer;
        transition: all 0.3s ease;
        font-size: 1rem;
    }

    button:hover {
        background-color: #007bff;
        color: white;
        border-color: #0056b3;
    }

    button:disabled {
        background: none;
        border: none;
        cursor: default;
    }

    button span {
        visibility: hidden;
    }
</style>

