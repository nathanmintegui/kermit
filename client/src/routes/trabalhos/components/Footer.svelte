<script lang="ts">
	const secoes = ['Trabalhos', 'Calendário'];

</script>

<div class="w-screen border-t absolute bottom-0 flex gap-5">
	<div class="flex gap-5 p-1">
		{#each secoes as secao }
			<p class="hover:cursor-pointer hover:font-bold pr-3">{secao}</p>
		{/each}
	</div>
</div>
